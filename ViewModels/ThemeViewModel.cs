namespace Grimoire.ViewModels;
using Grimoire.Models;
using Grimoire.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

public class ThemeViewModel : BindableObject
{
    private bool _isSelected;
    private Theme _currentTheme;
    public ThemeService ThemeService { get; }

    public ObservableCollection<ThemeViewModel> AvailableThemes { get; }

    public Theme CurrentTheme => _currentTheme;

    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool IsDark { get; set; }
    public Color? Primary { get; set; }
    public Color? Secondary { get; set; }
    public Color? Tertiary { get; set; }
    public Color? Background { get; set; }
    public Color? Surface { get; set; }
    public Color? OnPrimary { get; set; }
    public Color? OnSecondary { get; set; }
    public Color? OnTertiary { get; set; }
    public Color? OnBackground { get; set; }
    public bool IsSelected 
    { 
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SelectThemeCommand { get; set; }

    public ThemeViewModel()
    {
        _currentTheme = new Theme();
        ThemeService = new ThemeService();
        AvailableThemes = [.. ThemeService.AvailableThemes.Select(theme => new ThemeViewModel(ThemeService))];
        SelectThemeCommand = new AsyncRelayCommand(SelectTheme);
    }

    public ThemeViewModel(ThemeService themeService)
    {
        _currentTheme = themeService.CurrentTheme;
        ThemeService = themeService;       
        AvailableThemes = [.. ThemeService.AvailableThemes.Select(theme => new ThemeViewModel(ThemeService))];
        SelectThemeCommand = new AsyncRelayCommand(SelectTheme);
    }

    private async Task SelectTheme()
    {
        await ThemeService.SetThemeAsync(Id);
    }

}
