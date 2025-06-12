// ---------- Presentation/ViewModels/ArchiveViewModel.cs ----------
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grimoire.Domain.Models;
using Grimoire.Domain.UseCases;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grimoire.Presentation.ViewModels;

public partial class ArchiveViewModel : ObservableObject
{
    private readonly ILoadArchiveUseCase _loadArchive;
    private readonly ICreateNewTomeUseCase _createNewTome;
    private readonly ISaveEntryUseCase _saveEntry;
    private readonly IAddEntryToTomeUseCase _addEntryToTome;
    [ObservableProperty] private ObservableCollection<TomeViewModel> _allTomes = [];
    [ObservableProperty] private ObservableCollection<TomeEntry> _selectedTomeEntries = [];
    [ObservableProperty] private Tome? _selectedTome;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private TomeEntry? _selectedEntry;

    public ArchiveViewModel(ILoadArchiveUseCase loadArchive, ICreateNewTomeUseCase createNewTome, ISaveEntryUseCase saveEntry, IAddEntryToTomeUseCase addEntryToTome)
    {
        _loadArchive = loadArchive;
        _createNewTome = createNewTome;
        _saveEntry = saveEntry;
        _addEntryToTome = addEntryToTome;
    }

    [RelayCommand]
    private async Task InitializeAsync()
    {
        if (IsLoading) return;
        IsLoading = true;
        try
        {
            var response = await _loadArchive.ExecuteAsync(new());
            AllTomes = new ObservableCollection<TomeViewModel>(response.Tomes.Select(t => new TomeViewModel(_saveEntry, t)));
            SelectedTome = AllTomes.FirstOrDefault()?.Tome;
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CreateNewTomeAsync()
    {
        try
        {
            var response = await _createNewTome.ExecuteAsync(new CreateNewTomeRequest("Untitled"));
            AllTomes.Add(new TomeViewModel(_saveEntry, response.Tome));
            SelectedTome = response.Tome;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create new tome", ex);
        }
    }

    [RelayCommand]
    private async Task CreateNewEntryAsync()
    {
        try
        {
            var response = await _addEntryToTome.ExecuteAsync(SelectedTome!.Id, new TomeEntry(Guid.NewGuid().ToString(), "Untitled", "Untitled", DateTimeOffset.UtcNow, true));
            SelectedTomeEntries = [.. SelectedTomeEntries, response];
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create new entry", ex);
        }
    }

}
