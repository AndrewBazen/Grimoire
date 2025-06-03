using Entry = Grimoire.Models.Entry;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Grimoire.ViewModels;

public partial class EntryViewModel : ObservableObject, IQueryAttributable
{
    private Entry _entry;

    public string Content {
        get => _entry.Content;
        set
        {
            if (_entry.Content != value)
            {
                _entry.Content = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime CreatedAt => _entry.CreatedAt;

    public DateTime UpdatedAt => _entry.UpdatedAt;

    public string Id => _entry.Filename;

    public string Title {
        get => _entry.Title;
        set
        {
            if (_entry.Title != value)
            {
                _entry.Title = value;
                OnPropertyChanged();
            }
        }
    }
    

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }

    public EntryViewModel() 
    {
        _entry = new Entry();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public EntryViewModel(Entry entry)
    {
        _entry = entry;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        await _entry.Save();
        await Shell.Current.GoToAsync($"..?saved={_entry.Filename}");
    }

    private async Task Delete()
    {
        _entry.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_entry.Filename}");
    }

    public async Task Rename(string newTitle)
    {
        _entry.Title = newTitle;
        await _entry.Save();
        await Shell.Current.GoToAsync($"..?renamed={_entry.Filename}");
    }


    async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query) {
        if (query.TryGetValue("load", out object? value) && value != null) {
            if (value is string filename) {
                _entry = await Entry.LoadAsync(filename);
                RefreshProperties();
            }
            else {
                throw new Exception("Filename is null");
            }
        }
    }

    public async Task ReloadAsync() {
        _entry = await Entry.LoadAsync(_entry.Filename);
        RefreshProperties();
    }
    
    private void RefreshProperties() {
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Content));
        OnPropertyChanged(nameof(CreatedAt));
        OnPropertyChanged(nameof(UpdatedAt));
    }
    
    
}

