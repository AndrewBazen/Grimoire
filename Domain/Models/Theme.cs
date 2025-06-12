using CommunityToolkit.Mvvm.ComponentModel;

namespace Grimoire.Domain.Models;

public partial class Theme : ObservableObject
{
    // ───────── properties ─────────
    [ObservableProperty]
    private string _id = Guid.NewGuid().ToString();
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private bool _isSelected;
    // ───────── colors ─────────
    [ObservableProperty]
    private Color? _primary;
    [ObservableProperty]
    private Color? _secondary;
    [ObservableProperty]
    private Color? _tertiary;
    
    // ───────── background colors ─────────
    [ObservableProperty]
    private Color? _background;
    [ObservableProperty]
    private Color? _surface;
    
    // ───────── text colors ─────────
    [ObservableProperty]
    private Color? _onPrimary;
    [ObservableProperty]
    private Color? _onSecondary;
    [ObservableProperty]
    private Color? _onTertiary;
    [ObservableProperty]
    private Color? _onBackground;
    [ObservableProperty]
    private Color? _onSurface;
    
    // ───────── special colors ─────────
    [ObservableProperty]
    private Color? _error;
    [ObservableProperty]
    private Color? _success;
    [ObservableProperty]
    private Color? _warning;
    [ObservableProperty]
    private Color? _info;
    
    // ───────── ctor ─────────
    public Theme(
        string name,
        bool isSelected,
        Color primary,
        Color secondary,
        Color tertiary,
        Color background,
        Color surface,
        Color onPrimary,
        Color onSecondary,
        Color onTertiary,
        Color onBackground,
        Color onSurface,
        Color error,
        Color success,
        Color warning,
        Color info
    )
    {
        Name = name;
        IsSelected = isSelected;
        Primary = primary;
        Secondary = secondary;
        Tertiary = tertiary;
        Background = background;
        Surface = surface;
        OnPrimary = onPrimary;
        OnSecondary = onSecondary;
        OnTertiary = onTertiary;
        OnBackground = onBackground;
        OnSurface = onSurface;
        Error = error;
        Success = success;
        Warning = warning;
        Info = info;
    }
} 