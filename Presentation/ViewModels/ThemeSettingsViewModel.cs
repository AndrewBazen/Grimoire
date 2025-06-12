namespace Grimoire.Presentation.ViewModels;
using Grimoire.Domain.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Grimoire.Domain.UseCases;

public partial class ThemeSettingsViewModel : ObservableObject
{
    private readonly ILoadAvailableThemesUseCase _loadAvailableThemesUseCase;
    private readonly ISetThemeUseCase _setThemeUseCase;

    [ObservableProperty] private ObservableCollection<Theme> _availableThemes = [];

    [ObservableProperty] private Theme _currentTheme;

    partial void OnCurrentThemeChanged(Theme value)
    {
        if (value is null) return;
        _ = _setThemeUseCase.ExecuteAsync(value.Name);
    }

    public ThemeSettingsViewModel(ILoadAvailableThemesUseCase loadAvailableThemesUseCase, ISetThemeUseCase setThemeUseCase)
    {
        _loadAvailableThemesUseCase = loadAvailableThemesUseCase;
        _setThemeUseCase = setThemeUseCase;
        AvailableThemes = [.. _loadAvailableThemesUseCase.ExecuteAsync().Result];
        _currentTheme = AvailableThemes.FirstOrDefault(t => t.IsSelected) ?? AvailableThemes.First();
        CurrentTheme = _currentTheme;
        _setThemeUseCase.ExecuteAsync(_currentTheme.Name).Wait();
    }

    [RelayCommand]
    private async Task SelectThemeAsync()
    {
        await _setThemeUseCase.ExecuteAsync(CurrentTheme.Name);
    }

}
