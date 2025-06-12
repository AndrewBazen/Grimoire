using Grimoire.Domain.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace Grimoire.Presentation.ViewModels;

public partial class StartViewModel : ObservableObject
{
    private readonly IArchiveStore _store;
    [ObservableProperty] private ObservableCollection<IArchive> _archives = [];
    [ObservableProperty] private IArchive? _selectedArchive;
    [ObservableProperty] private bool _isLoading;

    public StartViewModel(IArchiveStore store)
    {
        _store = store;
    }

    [RelayCommand]
    private async Task InitializeAsync()
    {
        if (IsLoading) return;
        IsLoading = true;
        try
        {
            var archives = await _store.LoadAllAsync();
            Archives = new ObservableCollection<IArchive>(archives);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task CreateNewArchiveAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("New Archive", "Enter a name for your new archive:", "Create", "Cancel", "Untitled");
        if (string.IsNullOrWhiteSpace(name)) return;
        var archive = await _store.CreateAsync(name);
        Archives.Add(archive);
        SelectedArchive = archive;
        await Shell.Current.GoToAsync($"//archive/{archive.Id}");
    }

    [RelayCommand]
    private async Task OpenArchiveAsync()
    {
        if (SelectedArchive is null) return;
        await Shell.Current.GoToAsync($"/archive/{SelectedArchive.Id}");
    }

    [RelayCommand]
    private async Task RenameArchiveAsync(IArchive? archive)
    {
        if (archive is null) return;
        var newName = await Shell.Current.DisplayPromptAsync("Rename Archive", "Enter a new name:", "Save", "Cancel", archive.Name);
        if (string.IsNullOrWhiteSpace(newName) || newName == archive.Name) return;
        archive.Name = newName.Trim();
    }

    [RelayCommand]
    private async Task DeleteArchiveAsync(IArchive? archive)
    {
        if (archive is null) return;
        var confirm = await Shell.Current.DisplayAlert("Delete Archive", $"Are you sure you want to delete '{archive.Name}'? This cannot be undone.", "Delete", "Cancel");
        if (!confirm) return;
        Archives.Remove(archive);
        if (SelectedArchive == archive) SelectedArchive = null;
    }
}