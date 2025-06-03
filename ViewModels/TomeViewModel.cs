using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Entry = Grimoire.Models.Entry;
using System.Linq;

namespace Grimoire.ViewModels;

public partial class TomeViewModel : ObservableObject, IQueryAttributable
{
    public Models.Tome _tome;

    public string Id => _tome.Id;

    public string Name => _tome.Name;

    public DateTime CreatedAt => _tome.CreatedAt;

    public DateTime UpdatedAt => _tome.UpdatedAt;

    public string Author => _tome.Author;

    public ObservableCollection<EntryViewModel> AllEntries;

    public TomeViewModel()
    {
        _tome = new Models.Tome();
        AllEntries = new ObservableCollection<EntryViewModel>();
        _ = LoadEntriesAsync();
    }

    public TomeViewModel(Models.Tome tome)
    {
        _tome = tome;
        AllEntries = new ObservableCollection<EntryViewModel>();
        _ = LoadEntriesAsync();
    }
    
    private async Task LoadEntriesAsync()
    {
        var entries = await Entry.LoadAllAsync();
        foreach (var entry in entries)
        {
            AllEntries.Add(await CreateEntryViewModelAsync(entry.Filename));
        }
    }

    private async Task<EntryViewModel> CreateEntryViewModelAsync(string filename)
    {
        var entryViewModel = new EntryViewModel(await Entry.LoadAsync(filename));
        return entryViewModel;
    }

    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("deleted", out object? value))
        {
            string? entryId = value.ToString();
            EntryViewModel? matchingEntry = AllEntries.Where((e) => e.Id == entryId).FirstOrDefault();
            if (matchingEntry != null)
            {
                AllEntries.Remove(matchingEntry);
            }
        }
        else if (query.TryGetValue("saved", out object? savedValue))
        {
            string? entryId = savedValue.ToString();
            EntryViewModel? matchingEntry = AllEntries.Where((e) => e.Id == entryId).FirstOrDefault();
            if (matchingEntry != null)
            {
                await matchingEntry.ReloadAsync();
                AllEntries.Move(AllEntries.IndexOf(matchingEntry), 0);
            }
            else
            {
                if (entryId != null) {
                    var newEntry = await CreateEntryViewModelAsync(entryId);
                    AllEntries.Insert(0, newEntry);
                }
                else {
                    throw new Exception("Entry ID is null");
                }
            }
        }
 
    }
    
}

