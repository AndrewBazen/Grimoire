using Grimoire.Models;
using Entry = Grimoire.Models.Entry;

namespace Grimoire.Services.Interfaces;

public interface IArchive
{
    Task<bool> SaveTomeAsync(Tome tome);
    Task<Tome> LoadTomeAsync(string id);
    Task<List<Tome>> LoadAllTomesAsync();
    Task<bool> DeleteTomeAsync(string id);
    Task<bool> SaveEntryAsync(string tomeId, Entry entry);
    Task<List<Entry>> LoadEntriesAsync(string tomeId);
} 