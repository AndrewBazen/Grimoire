
namespace Grimoire.Domain.Models;

public sealed record Tome(
    string Id,
    string Name,
    string Author,
    IReadOnlyList<TomeEntry> Entries,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);