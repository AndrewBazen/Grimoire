using System.Windows.Input;
using Grimoire.Models;
using Grimoire.Services;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Grimoire.ViewModels;

public class ArchiveViewModel : IQueryAttributable
{
    public ObservableCollection<TomeViewModel> AllTomes { get; }
    public TomeService TomeService { get; }

    public ICommand CreateTomeCommand { get; }
    public ICommand DeleteTomeCommand { get; }
    public ICommand RenameTomeCommand { get; }
    public ICommand SelectTomeCommand { get; }

    public ArchiveViewModel()
    {
        TomeService = new TomeService(new LocalArchive());
        AllTomes = new ObservableCollection<TomeViewModel>();
        CreateTomeCommand = new AsyncRelayCommand(CreateTomeAsync);
        DeleteTomeCommand = new AsyncRelayCommand<TomeViewModel>(DeleteTomeAsync);
        RenameTomeCommand = new AsyncRelayCommand<TomeViewModel>(RenameTomeAsync);
        SelectTomeCommand = new AsyncRelayCommand<TomeViewModel>(SelectTomeAsync);

        _ = LoadTomesAsync();
    }

    public ArchiveViewModel(TomeService tomeService)
    {
        TomeService = tomeService;
        AllTomes = new ObservableCollection<TomeViewModel>();
        CreateTomeCommand = new AsyncRelayCommand(CreateTomeAsync);
        DeleteTomeCommand = new AsyncRelayCommand<TomeViewModel>(DeleteTomeAsync);
        RenameTomeCommand = new AsyncRelayCommand<TomeViewModel>(RenameTomeAsync);
        SelectTomeCommand = new AsyncRelayCommand<TomeViewModel>(SelectTomeAsync);

        _ = LoadTomesAsync();
    }

    private async Task LoadTomesAsync()
    {
        var tomes = await TomeService.LoadAllTomesAsync();
        foreach (var tome in tomes)
        {
            AllTomes.Add(new TomeViewModel(tome));
        }
    }

    private async Task CreateTomeAsync()
    {
        var tome = await TomeService.CreateTomeAsync();
        AllTomes.Add(new TomeViewModel(tome));
    }

    private async Task CreateTomeAsync(string name, string description)
    {
        var tome = await TomeService.CreateTomeAsync(name, description);
        AllTomes.Add(new TomeViewModel(tome));
    }

    private async Task DeleteTomeAsync(TomeViewModel? tome)
    {
        if (tome != null)
        {
            await TomeService.DeleteTomeAsync(tome.Id);
            AllTomes.Remove(tome);
        }
    }

    private async Task RenameTomeAsync(TomeViewModel? tome)
    {
        if (tome != null)
        {
            // You'll need to get the new name from UI or pass it differently
            // For now, using a placeholder
            await TomeService.RenameTomeAsync(tome.Id, "New Name");
        }
    }

    private async Task SelectTomeAsync(TomeViewModel? tome)
    {
        if (tome != null)
        {
            await Shell.Current.GoToAsync($"..?load={tome.Id}");
        }
    }
    
    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("load", out object? value) && value != null)
        {
            if (value is string filename) {
                var tome = await TomeService.LoadTomeAsync(filename);
                if (tome != null) {
                    AllTomes.Add(new TomeViewModel(tome));
                }
            }
            else {
                throw new Exception("Filename is null");
            }
        }
    }
    
    
    
}
