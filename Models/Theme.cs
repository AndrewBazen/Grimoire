
namespace Grimoire.Models;

public class Theme
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public bool IsDark { get; set; }
    
    // Primary colors
    public Color? Primary { get; set; }
    public Color? Secondary { get; set; }
    public Color? Tertiary { get; set; }
    
    // Background colors
    public Color? Background { get; set; }
    public Color? Surface { get; set; }
    
    // Text colors
    public Color? OnPrimary { get; set; }
    public Color? OnSecondary { get; set; }
    public Color? OnTertiary { get; set; }
    public Color? OnBackground { get; set; }
    public Color? OnSurface { get; set; }
    
    // Special colors
    public Color? Error { get; set; }
    public Color? Success { get; set; }
    public Color? Warning { get; set; }
    public Color? Info { get; set; }
    
    public Theme()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Default";
        IsDark = false;
    }
    
    // Predefined themes
    public static Theme Light => new Theme
    {
        Id = "light",
        Name = "Light",
        IsDark = false,
        Primary = Colors.Purple,
        Secondary = Colors.Indigo,
        Tertiary = Colors.Teal,
        Background = Colors.White,
        Surface = Color.FromArgb("#f5f5f5"),
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnTertiary = Colors.White,
        OnBackground = Colors.Black,
        OnSurface = Colors.Black,
        Error = Colors.Red,
        Success = Colors.Green,
        Warning = Colors.Orange,
        Info = Colors.Blue
    };
    
    public static Theme Dark => new Theme
    {
        Id = "dark",
        Name = "Dark",
        IsDark = true,
        Primary = Colors.Purple,
        Secondary = Colors.Indigo,
        Tertiary = Colors.Teal,
        Background = Color.FromArgb("#121212"),
        Surface = Color.FromArgb("#1e1e1e"),
        OnPrimary = Colors.White,
        OnSecondary = Colors.White,
        OnTertiary = Colors.White,
        OnBackground = Colors.White,
        OnSurface = Colors.White,
        Error = Colors.Red,
        Success = Colors.Green,
        Warning = Colors.Orange,
        Info = Colors.Blue
    };
} 