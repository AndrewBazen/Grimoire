using Grimoire.Domain.Ports;

using Grimoire.Infrastructure.Implementations;
using Grimoire.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Grimoire.Infrastructure.Persistence;

public class LocalArchiveStore : IArchiveStore
{
    private readonly ConcurrentDictionary<string, IArchive> _archives = new();
    private readonly IServiceProvider _sp;

    public LocalArchiveStore(IServiceProvider sp)
    {
        _sp = sp;
    }

    public Task<IReadOnlyList<IArchive>> LoadAllAsync(CancellationToken ct = default)
    {
        IReadOnlyList<IArchive> list = _archives.Values.ToList();
        return Task.FromResult(list);
    }

    public Task<IArchive> CreateAsync(string name, CancellationToken ct = default)
    {
        var archive = ActivatorUtilities.CreateInstance<LocalArchive>(_sp);
        archive.Name = name;
        _archives[archive.Id] = archive;
        return Task.FromResult<IArchive>(archive);
    }

    public Task<IArchive> LoadAsync(string id, CancellationToken ct = default)
    {
        if (_archives.TryGetValue(id, out var archive)) return Task.FromResult(archive);
        throw new KeyNotFoundException($"Archive {id} not found");
    }

    public Task<IArchive> SaveAsync(IArchive archive, CancellationToken ct = default)
    {
        _archives[archive.Id] = archive;
        return Task.FromResult(archive);
    }

    public Task<IArchive> DeleteAsync(string id, CancellationToken ct = default)
    {
        if (_archives.TryRemove(id, out var removed)) return Task.FromResult(removed);
        throw new KeyNotFoundException($"Archive {id} not found");
    }
}

