using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Grimoire.Services;

public class ThemeService
{
    private const string ThemePreferenceKey = "current_theme";
    private readonly List<Theme> _availableThemes = new();
    private Theme _currentTheme;
    
    public event EventHandler<Theme>? ThemeChanged;
    
    public ThemeService()
    {
        // Add default themes
        _availableThemes.Add(Theme.Light);
        _availableThemes.Add(Theme.Dark);
        
        // Set default theme
        _currentTheme = Theme.Light;
    }
    
    public Theme CurrentTheme => _currentTheme;
    
    public List<Theme> AvailableThemes => _availableThemes;
    
    public void AddTheme(Theme theme)
    {
        if (!_availableThemes.Any(t => t.Id == theme.Id))
        {
            _availableThemes.Add(theme);
        }
    }
    
    public async Task InitializeAsync()
    {
        await LoadSavedThemeAsync();
    }
    
    public async Task SetThemeAsync(string themeId)
    {
        var theme = _availableThemes.FirstOrDefault(t => t.Id == themeId);
        if (theme != null)
        {
            await SetThemeAsync(theme);
        }
    }
    
    public async Task SetThemeAsync(Theme theme)
    {
        if (theme == null) return;
        
        _currentTheme = theme;
        
        if (Application.Current != null) {
            // Apply theme to app resources
            Application.Current.Resources["Primary"] = theme.Primary;
            Application.Current.Resources["Secondary"] = theme.Secondary;
            Application.Current.Resources["Tertiary"] = theme.Tertiary;
            
            Application.Current.Resources["Background"] = theme.Background;
            Application.Current.Resources["Surface"] = theme.Surface;
        
            Application.Current.Resources["OnPrimary"] = theme.OnPrimary;
            Application.Current.Resources["OnSecondary"] = theme.OnSecondary;
            Application.Current.Resources["OnTertiary"] = theme.OnTertiary;
            Application.Current.Resources["OnBackground"] = theme.OnBackground;
            Application.Current.Resources["OnSurface"] = theme.OnSurface;
            
            Application.Current.Resources["Error"] = theme.Error;
            Application.Current.Resources["Success"] = theme.Success;
            Application.Current.Resources["Warning"] = theme.Warning;
            Application.Current.Resources["Info"] = theme.Info;
            
            // Set system theme
            Application.Current.UserAppTheme = theme.IsDark ? AppTheme.Dark : AppTheme.Light;
        }
        
        // Save preference
        await SaveThemePreferenceAsync(theme.Id);
        
        // Notify listeners
        ThemeChanged?.Invoke(this, theme);
    }
    
    private async Task SaveThemePreferenceAsync(string themeId)
    {
        try
        {
            await SecureStorage.SetAsync(ThemePreferenceKey, themeId);
        }
        catch (Exception)
        {
            // Fallback for platforms that don't support secure storage
            Preferences.Set(ThemePreferenceKey, themeId);
        }
    }
    
    private async Task LoadSavedThemeAsync()
    {
        string? savedThemeId;
        try
        {
            savedThemeId = await SecureStorage.GetAsync(ThemePreferenceKey);
        }
        catch (Exception)
        {
            if (Application.Current != null) {
                // Fallback for platforms that don't support secure storage
                savedThemeId = Preferences.Get(ThemePreferenceKey, null);
            } else {
                savedThemeId = null;
            }
        }
        
        if (!string.IsNullOrEmpty(savedThemeId))
        {
            await SetThemeAsync(savedThemeId);
        }
        else
        {
            if (Application.Current != null) {
                // Default to system preference, or Light if no system preference
                bool isDarkMode = Application.Current.RequestedTheme == AppTheme.Dark;
                await SetThemeAsync(isDarkMode ? Theme.Dark.Id : Theme.Light.Id);
            } else {
                await SetThemeAsync(Theme.Light.Id);
            }
        }
    }
} 