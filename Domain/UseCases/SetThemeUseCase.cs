using Grimoire.Domain.Services;

namespace Grimoire.Domain.UseCases;

public interface ISetThemeUseCase
{
    Task ExecuteAsync(string themeName);
}

public class SetThemeUseCase : ISetThemeUseCase
{
    private readonly ThemeService _themeService;

    public SetThemeUseCase(ThemeService themeService)
    {
        _themeService = themeService;
    }

    public async Task ExecuteAsync(string themeName)
    {
        await _themeService.SetTheme(themeName);
    }
}