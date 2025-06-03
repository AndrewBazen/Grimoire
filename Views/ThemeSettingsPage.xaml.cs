using System.Collections.ObjectModel;
using Grimoire.Models;
using Grimoire.Services;
using Grimoire.ViewModels;
using Microsoft.Maui.Controls;

namespace Grimoire.Views;

public partial class ThemeSettingsPage : ContentPage
{
    private readonly ThemeService _themeService;
    private readonly ObservableCollection<ThemeViewModel> _themes = new();

    public ThemeSettingsPage(ThemeService themeService)
    {
        InitializeComponent();
        _themeService = themeService;

        // Initialize themes collection
        ThemesCollection.ItemsSource = _themes;
        
        // Load available themes
        LoadThemes();
    }

    private void LoadThemes()
    {
        _themes.Clear();
        
        foreach (var theme in _themeService.AvailableThemes)
        {
            _themes.Add(new ThemeViewModel
            {
                Id = theme.Id,
                Name = theme.Name,
                IsDark = theme.IsDark,
                Primary = theme.Primary,
                Background = theme.Background,
                Surface = theme.Surface,
                OnPrimary = theme.OnPrimary,
                OnSecondary = theme.OnSecondary,
                OnTertiary = theme.OnTertiary,
                OnBackground = theme.OnBackground,
                Tertiary = theme.Tertiary,
                Secondary = theme.Secondary,
                IsSelected = theme.Id == _themeService.CurrentTheme.Id
            });
        }
    }
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // Get TomeService from dependency injection
        var themeService = Handler?.MauiContext?.Services?.GetService<ThemeService>();
        
        if (themeService != null)
        {
            var viewModel = new ThemeViewModel();
            BindingContext = viewModel;
        }
        else
        {
            throw new Exception("ThemeService is null");
        }
    }
    private async void OnThemeRadioButtonChecked(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return;

        if (sender is RadioButton radioButton && radioButton.BindingContext is ThemeViewModel selectedTheme)
        {
            // Update selected state in the view models
            foreach (var theme in _themes)
            {
                theme.IsSelected = theme.Id == selectedTheme.Id;
            }

            // Apply the theme
            if (selectedTheme.Id != null)
            {
                await _themeService.SetThemeAsync(selectedTheme.Id);
            }
            else
            {
                await _themeService.SetThemeAsync(Theme.Light.Id);
            }
        }
    }
}

