
// ---------- Domain/UseCases/LoadArchiveUseCase.cs ----------
using Grimoire.Domain.Models;
using Grimoire.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace Grimoire.Domain.UseCases;

/// <summary>
/// Request DTO (no parameters yet – kept for future filters/pagination).
/// </summary>
public sealed record LoadArchiveRequest;

/// <summary>
/// Response DTO wrapping the read‑only list of Tomes.
/// </summary>
/// <param name="Tomes">All tomes belonging to the current archive.</param>
public sealed record LoadArchiveResponse(IReadOnlyList<Tome> Tomes);

/// <summary>
/// Production implementation that queries the persistence port (IArchive) and converts to DTO.
/// </summary>
public sealed class LoadArchiveUseCase : ILoadArchiveUseCase
{
    private readonly IArchive _archive;
    private readonly ILogger<LoadArchiveUseCase> _logger;

    public LoadArchiveUseCase(IArchive archive, ILogger<LoadArchiveUseCase> logger)
    {
        _archive = archive;
        _logger = logger;
    }

    public async Task<LoadArchiveResponse> ExecuteAsync(LoadArchiveRequest request, CancellationToken ct = default)
    {
        try
        {
            var tomes = await _archive.LoadAllTomesAsync(ct);
            return new LoadArchiveResponse(tomes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load archive");
            throw; // Bubble up; VM decides how to surface error to the UI.
        }
    }
}