using Grimoire.Domain.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grimoire.Domain.UseCases;
using System.Collections.ObjectModel;

namespace Grimoire.Presentation.ViewModels;

public partial class WorkspaceViewModel : ObservableObject
{
    [ObservableProperty] private IArchive _currentArchive;
    [ObservableProperty] private ArchiveViewModel _archiveViewModel;
    [ObservableProperty] private ObservableCollection<Grimoire.Domain.Models.TomeEntry> _selectedTomeEntries;
    [ObservableProperty] private Grimoire.Domain.Models.TomeEntry? _selectedEntry;
    [ObservableProperty] private string _archiveName;

    private readonly ILoadArchiveUseCase _loadArchive;
    private readonly ICreateNewTomeUseCase _createNewTome;
    private readonly ISaveEntryUseCase _saveEntry;
    private readonly IAddEntryToTomeUseCase _addEntryToTome;

    public WorkspaceViewModel(IArchive selectedArchive, ILoadArchiveUseCase loadArchive, ICreateNewTomeUseCase createNewTome, ISaveEntryUseCase saveEntry, IAddEntryToTomeUseCase addEntryToTome)
    {
        _currentArchive = selectedArchive;
        _loadArchive = loadArchive;
        _createNewTome = createNewTome;
        _saveEntry = saveEntry;
        _addEntryToTome = addEntryToTome;
        _archiveViewModel = new ArchiveViewModel(_loadArchive, _createNewTome, _saveEntry, _addEntryToTome);
        _selectedTomeEntries = new ObservableCollection<Grimoire.Domain.Models.TomeEntry>();
        _archiveName = selectedArchive.Name;
    }

    [RelayCommand]
    private async Task CreateNewTomeAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("New Tome", "Enter a name for your new tome:", "Create", "Cancel", "Untitled");
        if (string.IsNullOrWhiteSpace(name)) return;
        await ArchiveViewModel.CreateNewTomeCommand.ExecuteAsync(name);
    }

    [RelayCommand]
    private async Task CreateNewEntryAsync()
    {
        var title = await Shell.Current.DisplayPromptAsync("New Entry", "Enter a title for your new entry:", "Create", "Cancel", "Untitled");
        if (string.IsNullOrWhiteSpace(title)) return;
        await ArchiveViewModel.CreateNewEntryCommand.ExecuteAsync(title);
    }

    [RelayCommand]
    private void SelectEntry(Grimoire.Domain.Models.TomeEntry entry)
    {
        SelectedEntry = entry;
    }
}