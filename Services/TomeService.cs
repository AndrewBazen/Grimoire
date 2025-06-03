/* @TomeService.cs
 *
 * This file contains the TomeService class, which is responsible for managing the Tome archive.
 * It provides methods for creating, updating, and deleting tomes, as well as adding and removing entries.
 *
 * @author Andrew Bazen
 * @date 2025-06-03
 * @version 0.1.0
 */
using Grimoire.Models;
using Entry = Grimoire.Models.Entry;
using Grimoire.Services.Interfaces;

namespace Grimoire.Services;

public class TomeService(IArchive archive)
{
    private readonly IArchive _archive = archive;

    public async Task<Tome> CreateTomeAsync()
    {
        var tome = new Tome
        {
            Name = "Untitled Tome",
            Description = "Enter a description for your tome here.",
            Author = Environment.UserName,
            Language = Environment.OSVersion.Platform.ToString(),
            Theme = "current_theme",
            Entries = []
        };

        await _archive.SaveTomeAsync(tome);
        return tome;
    }

    public async Task<Tome> CreateTomeAsync(string name, string description)
    {
        var tome = new Tome
        {
            Name = name,
            Description = description,
            Author = Environment.UserName,
            Language = Environment.OSVersion.Platform.ToString(),
            Theme = "current_theme",
            Entries = []
        };

        await _archive.SaveTomeAsync(tome);
        return tome;
    }

    public async Task<List<Tome>> LoadAllTomesAsync()
    {
        var tomes = await _archive.LoadAllTomesAsync();
        if (tomes == null)
        {
            throw new Exception("No tomes found");
        } 
        else {
            return tomes.OrderByDescending(static t => t.UpdatedAt).ToList();
        }   
    }

    public async Task<Tome> LoadTomeAsync(string id)
    {
        var tome = await _archive.LoadTomeAsync(id) ?? throw new Exception($"Tome with ID {id} not found");
        return tome;
    }

    public async Task<bool> SaveTomeAsync(Tome tome)
    {
        tome.UpdatedAt = DateTime.Now;
        await _archive.SaveTomeAsync(tome);
        return true;
    }

    public async Task<bool> DeleteTomeAsync(string id)
    {
        await _archive.DeleteTomeAsync(id);
        return true;
    }

    public async Task<Grimoire.Models.Entry> AddEntry(string tomeId, string title, string content)
    {
        var tome = await _archive.LoadTomeAsync(tomeId);
        if (tome == null)
        {
            throw new Exception($"Tome with ID {tomeId} not found");
        }

        var entry = new Grimoire.Models.Entry
        {
            Title = title,
            Content = content
        };

        tome.Entries.Add(entry);
        await _archive.SaveTomeAsync(tome);
        await _archive.SaveEntryAsync(tomeId, entry);

        return entry;
    }

    public async Task<bool> UpdateEntry(string tomeId, Entry entry)
    {
        var tome = await _archive.LoadTomeAsync(tomeId);
        if (tome == null)
        {
            throw new Exception($"Tome with ID {tomeId} not found");
        }

        entry.UpdatedAt = DateTime.Now;
        
        // Update entry in the tome's entries list
        var existingEntryIndex = tome.Entries.IndexOf(entry);
        if (existingEntryIndex >= 0)
        {
            tome.Entries[existingEntryIndex] = entry;
        }

        await _archive.SaveTomeAsync(tome);
        await _archive.SaveEntryAsync(tomeId, entry);

        return true;
    }

    public async Task<bool> RemoveEntry(string tomeId, string entryId)
    {
        var tome = await _archive.LoadTomeAsync(tomeId);
        if (tome == null)
        {
            throw new Exception($"Tome with ID {tomeId} not found");
        }

        var entryToRemove = tome.Entries.FirstOrDefault(e => e.Filename == entryId);
        if (entryToRemove == null)
        {
            throw new Exception($"Entry with ID {entryId} not found in Tome {tomeId}");
        }

        tome.Entries.Remove(entryToRemove);
        await _archive.SaveTomeAsync(tome);

        return true;
    }

    public async Task<bool> RenameTomeAsync(string tomeId, string newName)
    {
        var tome = await _archive.LoadTomeAsync(tomeId) ?? throw new Exception($"Tome with ID {tomeId} not found");
        tome.Name = newName;
        await _archive.SaveTomeAsync(tome);
        return true;
    }

    public async Task<List<Entry>> GetAllEntries(string tomeId)
    {
        return await _archive.LoadEntriesAsync(tomeId);
    }
} 