using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grimoire.Domain.Ports;
using System.Collections.ObjectModel;

namespace Grimoire.Presentation.ViewModels;

public partial class ArchiveStoreViewModel : ObservableObject
{
    private readonly IArchiveStore _store;
    [ObservableProperty] private ObservableCollection<IArchive> _archives = [];
    [ObservableProperty] private IArchive? _currentArchive;
    public ArchiveStoreViewModel(IArchiveStore store)
    {
        _store = store;
    }

    [RelayCommand]
    private async Task InitializeAsync()
    {
        Archives = [.. await _store.LoadAllAsync()];
        // set the current archive to the last archive that updated
        CurrentArchive = Archives.OrderByDescending(a => a.UpdatedAt).FirstOrDefault();
    }
}