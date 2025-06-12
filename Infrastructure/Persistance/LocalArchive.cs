// ---------- Infrastructure/Persistence/LocalArchive.cs ----------
// (Replaces previous version – now includes Entry CRUD)
using System.Collections.Concurrent;
using Grimoire.Domain.Models;
using Grimoire.Domain.Ports;
using Grimoire.Infrastructure.Abstractions;
using Grimoire.Infrastructure.Implementations;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace Grimoire.Infrastructure.Persistence;

public sealed partial class LocalArchive : ObservableObject, IArchive, IDisposable
{
    [ObservableProperty] private string _id = Guid.NewGuid().ToString();
    [ObservableProperty] private string _name = "Untitled";
    [ObservableProperty] private string _author = Environment.UserName;
    [ObservableProperty] private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
    [ObservableProperty] private DateTimeOffset _updatedAt = DateTimeOffset.UtcNow;

    private readonly FileSystemAdapter _fs;
    private readonly SystemTextJsonSerializer _json;
    private readonly AppPathProvider _paths;
    private readonly ILogger<LocalArchive> _logger;

    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();
    private readonly SemaphoreSlim _indexLock = new(1, 1);

    public LocalArchive(FileSystemAdapter fs, SystemTextJsonSerializer json, AppPathProvider paths, ILogger<LocalArchive> logger)
    {
        _fs = fs; _json = json; _paths = paths; _logger = logger;
    }

    // ----------------- Archive/Tome APIs (unchanged) -----------------

    public async Task<IReadOnlyList<Tome>> LoadAllTomesAsync(CancellationToken ct = default)
    {
        await _indexLock.WaitAsync(ct);
        try
        {
            var files = _fs.GetFiles(_paths.ArchiveRoot, "*.tome.json");
            var tomes = new List<Tome>(files.Length);
            foreach (var file in files)
            {
                var id = Path.GetFileNameWithoutExtension(file).Replace(".tome", "", StringComparison.Ordinal);
                var tome = await LoadTomeUnsafeAsync(id, ct);
                if (tome is not null) tomes.Add(tome);
            }
            return tomes;
        }
        finally { _indexLock.Release(); }
    }

    public async Task<Tome?> LoadTomeAsync(string id, CancellationToken ct = default)
    {
        var slim = _locks.GetOrAdd(id, _ => new(1,1));
        await slim.WaitAsync(ct);
        try { return await LoadTomeUnsafeAsync(id, ct); }
        finally { slim.Release(); }
    }

    public async Task SaveTomeAsync(Tome tome, CancellationToken ct = default)
    {
        var slim = _locks.GetOrAdd(tome.Id, _ => new(1,1));
        await slim.WaitAsync(ct);
        try
        {
            var json = _json.Serialize(tome);
            await _fs.WriteAllTextAsync(_paths.TomePath(tome.Id), json, ct);
        }
        finally { slim.Release(); }
    }

    // ----------------- Entry‑level APIs -----------------

    public async Task<TomeEntry?> LoadEntryAsync(string tomeId, string entryId, CancellationToken ct = default)
    {
        var tome = await LoadTomeAsync(tomeId, ct);
        return tome?.Entries.FirstOrDefault(e => e.Id.Equals(entryId));
    }

    public async Task SaveEntryAsync(string tomeId, TomeEntry entry, CancellationToken ct = default)
    {
        var slim = _locks.GetOrAdd(tomeId, _ => new(1,1));
        await slim.WaitAsync(ct);
        try
        {
            var tome = await LoadTomeUnsafeAsync(tomeId, ct) ?? new Tome(tomeId, "Untitled", "Unknown", [], DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
            var list = tome.Entries.ToList();
            var idx = list.FindIndex(e => e.Id.Equals(entry.Id));
            if (idx >= 0) list[idx] = entry; else list.Add(entry);
            await SaveTomeAsync(tome with { Entries = list }, ct);
        }
        finally { slim.Release(); }
    }

    public async Task DeleteEntryAsync(string tomeId, string entryId, CancellationToken ct = default)
    {
        var slim = _locks.GetOrAdd(tomeId, _ => new(1,1));
        await slim.WaitAsync(ct);
        try
        {
            var tome = await LoadTomeUnsafeAsync(tomeId, ct);
            if (tome is null) return;
            var newEntries = tome.Entries.Where(e => e.Id != entryId).ToList();
            await SaveTomeAsync(tome with { Entries = newEntries }, ct);
        }
        finally { slim.Release(); }
    }

    public void Dispose()
    {
        _indexLock.Dispose();
        foreach (var s in _locks.Values) s.Dispose();
        _locks.Clear();
    }

    // ----------------- Helpers -----------------

    private async Task<Tome?> LoadTomeUnsafeAsync(string id, CancellationToken ct)
    {
        try
        {
            var path = _paths.TomePath(id);
            if (!_fs.FileExists(path)) return null;
            var json = await _fs.ReadAllTextAsync(path, ct);
            return _json.Deserialize<Tome>(json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load tome {TomeId}", id);
            return null;
        }
    }

    public async Task SaveMetadataAsync(string id)
    {
        var meta = new { Name, Author, CreatedAt, UpdatedAt = DateTimeOffset.UtcNow };
        var json = JsonConvert.SerializeObject(meta);
        await _fs.WriteAllTextAsync(_paths.ArchiveMetaPath(id), json);
    }
}