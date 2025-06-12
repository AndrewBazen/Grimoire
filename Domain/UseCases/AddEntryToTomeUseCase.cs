using Grimoire.Domain.Models;
using Grimoire.Domain.Ports;

namespace Grimoire.Domain.UseCases;

public interface IAddEntryToTomeUseCase
{
    Task<TomeEntry> ExecuteAsync(string tomeId, TomeEntry entry, CancellationToken ct = default);
}

public class AddEntryToTomeUseCase : IAddEntryToTomeUseCase
{
    private readonly IArchive _archive;

    public AddEntryToTomeUseCase(IArchive archive)
    {
        _archive = archive;
    }
    public async Task<TomeEntry> ExecuteAsync(string tomeId, TomeEntry entry, CancellationToken ct = default)
    {
        await _archive.SaveEntryAsync(tomeId, entry, ct);
        return entry;
    }

}