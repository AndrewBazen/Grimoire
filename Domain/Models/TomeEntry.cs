
namespace Grimoire.Domain.Models
{
    /// <summary>
    /// Immutable note inside a Tome.
    /// </summary>
    public sealed record TomeEntry(
        string Id,
        string Title,
        string Content,
        DateTimeOffset UpdatedUtc,
        bool IsSelected = false);

}
