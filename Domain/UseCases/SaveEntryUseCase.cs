// ---------- Domain/UseCases/SaveEntryUseCase.cs ----------
using Grimoire.Domain.Models;
using Grimoire.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace Grimoire.Domain.UseCases;

/// <summary>
/// Request DTO for saving an entry.
/// </summary>
/// <param name="TomeId">The ID of the tome containing the entry.</param>
/// <param name="Entry">The entry to save.</param>
public sealed record SaveEntryRequest(string TomeId, TomeEntry Entry);

public sealed record SaveEntryResponse(TomeEntry Entry);

public interface ISaveEntryUseCase
{
    Task<SaveEntryResponse> ExecuteAsync(SaveEntryRequest request, CancellationToken ct = default);
}

public sealed class SaveEntryUseCase : ISaveEntryUseCase
{
    private readonly IArchive _archive;
    private readonly ILogger<SaveEntryUseCase> _logger;

    public SaveEntryUseCase(IArchive archive, ILogger<SaveEntryUseCase> logger)
    {
        _archive = archive;
        _logger = logger;
    }

    public async Task<SaveEntryResponse> ExecuteAsync(SaveEntryRequest request, CancellationToken ct = default)
    {
        var tome = await _archive.LoadTomeAsync(request.TomeId, ct) ??
                   throw new InvalidOperationException($"Tome '{request.TomeId}' not found.");

        // Replace existing entry or append new.
        var list = tome.Entries.ToList();
        var idx  = list.FindIndex(e => e.Id.Equals(request.Entry.Id));
        if (idx >= 0)
            list[idx] = request.Entry with { UpdatedUtc = DateTimeOffset.UtcNow };
        else
            list.Add(request.Entry with { UpdatedUtc = DateTimeOffset.UtcNow });

        var updated = tome with { Entries = list };
        await _archive.SaveTomeAsync(updated, ct);
        return new SaveEntryResponse(request.Entry);
    }
}