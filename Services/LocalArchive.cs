using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Models;
using Grimoire.Services.Interfaces;

namespace Grimoire.Services;

public class LocalArchive : IArchive
{
    private readonly string _baseDirectory;

    public LocalArchive()
    {
        _baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Grimoire", "Tomes");
        EnsureDirectoryExists(_baseDirectory);
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public async Task<bool> SaveTomeAsync(Tome tome)
    {
        string tomePath = Path.Combine(_baseDirectory, $"{tome.Id}.json");
        string entriesPath = Path.Combine(_baseDirectory, tome.Id, "entries");
        
        EnsureDirectoryExists(Path.Combine(_baseDirectory, tome.Id));
        EnsureDirectoryExists(entriesPath);
        
        await File.WriteAllTextAsync(tomePath, JsonSerializer.Serialize(tome));
        return true;
    }

    public async Task<Tome> LoadTomeAsync(string id)
    {
        string path = Path.Combine(_baseDirectory, $"{id}.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Tome with id {id} not found");
        }
        
        var json = await File.ReadAllTextAsync(path);
        var tome = JsonSerializer.Deserialize<Tome>(json) ?? throw new InvalidOperationException($"Failed to deserialize tome with id {id}");
        return tome;
    }

    public async Task<List<Tome>> LoadAllTomesAsync()
    {
        List<Tome> tomes = new List<Tome>();
        
        if (!Directory.Exists(_baseDirectory))
        {
            return tomes;
        }
        
        foreach (var file in Directory.GetFiles(_baseDirectory, "*.json"))
        {
            var json = await File.ReadAllTextAsync(file);
            var tome = JsonSerializer.Deserialize<Tome>(json);
            if (tome != null)
            {
                tomes.Add(tome);
            }
        }
        
        return tomes;
    }

    public async Task<bool> DeleteTomeAsync(string id)
    {
        string tomePath = Path.Combine(_baseDirectory, $"{id}.json");
        string tomeDirectory = Path.Combine(_baseDirectory, id);
        
        if (File.Exists(tomePath))
        {
            File.Delete(tomePath);
        }
        
        if (Directory.Exists(tomeDirectory))
        {
            Directory.Delete(tomeDirectory, true);
        }
        
        return await Task.FromResult(true);
    }

    public async Task<bool> SaveEntryAsync(string tomeId, Models.Entry entry)
    {
        string entriesPath = Path.Combine(_baseDirectory, tomeId, "entries");
        EnsureDirectoryExists(entriesPath);
        
        string entryPath = Path.Combine(entriesPath, $"{entry.Filename}");
        await File.WriteAllTextAsync(entryPath, entry.Content);
        
        return true;
    }

    public async Task<List<Models.Entry>> LoadEntriesAsync(string tomeId)
    {
        List<Models.Entry> entries = new List<Models.Entry>();
        string entriesPath = Path.Combine(_baseDirectory, tomeId, "entries");
        
        if (!Directory.Exists(entriesPath))
        {
            return entries;
        }
        
        foreach (var file in Directory.GetFiles(entriesPath, "*.md"))
        {
            var entry = await Models.Entry.LoadAsync(Path.GetFileName(file));
            if (entry != null)
            {
                entries.Add(entry);
            }
        }
        
        return entries;
    }
} 