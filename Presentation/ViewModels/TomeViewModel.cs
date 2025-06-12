// ---------- Presentation/ViewModels/TomeViewModel.cs ----------
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Grimoire.Domain.Models;
using Grimoire.Domain.UseCases;

namespace Grimoire.Presentation.ViewModels;

public partial class TomeViewModel : ObservableObject
{

    [ObservableProperty]
    private Tome _tome;

    [ObservableProperty]
    private ObservableCollection<EntryViewModel> _entries;

    public TomeViewModel()
    {
        // Initialize a new empty tome and its entries collection
        _tome = new Tome(Guid.NewGuid().ToString(), "New Tome", "New Tome", [], DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        _entries = [];
    }

    public TomeViewModel(ISaveEntryUseCase saveEntry, Tome tome)
    {
        _tome = tome;
        // Load all entries for the tome if there are any
        if (tome.Entries != null && tome.Entries.Count > 0)
        {
            _entries = [.. tome.Entries.Select(e => new EntryViewModel(saveEntry, tome.Id, e.Id))];
        } 
        else // If there are no entries, initialize an empty collection
        {
            _entries = [];
        }
    }

}
