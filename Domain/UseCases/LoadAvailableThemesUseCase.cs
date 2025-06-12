using Grimoire.Domain.Models;
using Grimoire.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Grimoire.Domain.UseCases;

public interface ILoadAvailableThemesUseCase
{
    Task<IEnumerable<Theme>> ExecuteAsync();
}

public class LoadAvailableThemesUseCase : ILoadAvailableThemesUseCase
{
    private readonly ILogger<LoadAvailableThemesUseCase> _logger;
    private readonly ThemeService _themeService;
    public LoadAvailableThemesUseCase(ILogger<LoadAvailableThemesUseCase> logger, ThemeService themeService)
    {
        _logger = logger;
        _themeService = themeService;
    }
    public async Task<IEnumerable<Theme>> ExecuteAsync()
    {
        _logger.LogInformation("Loading available themes");
        return await Task.FromResult(_themeService.Available);
    }

}

