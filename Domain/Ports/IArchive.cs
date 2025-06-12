// ---------- Domain/Ports/IArchive.cs (for reference) ----------
using Grimoire.Domain.Models;
namespace Grimoire.Domain.Ports;

public interface IArchive
{
    // ───────── Archive properties ─────────
    string Id { get; set; }
    string Name { get; set; }
    string Author { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }

    // ───────── Tome APIs ─────────
    Task<IReadOnlyList<Tome>> LoadAllTomesAsync(CancellationToken ct = default);
    Task<Tome?> LoadTomeAsync(string id, CancellationToken ct = default);
    Task SaveTomeAsync(Tome tome, CancellationToken ct = default);

    // ───────── Entry APIs ─────────
    Task<TomeEntry?> LoadEntryAsync(string tomeId, string entryId, CancellationToken ct = default);
    Task SaveEntryAsync(string tomeId, TomeEntry entry, CancellationToken ct = default);
    Task DeleteEntryAsync(string tomeId, string entryId, CancellationToken ct = default);
}
