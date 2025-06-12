// ---------- Infrastructure/Abstractions/IPathProvider.cs ----------
namespace Grimoire.Infrastructure.Abstractions;

public interface IPathProvider
{
    string ArchiveRoot { get; }
    string TomePath(string tomeId);
    string EntryPath(string entryId);
    string ArchiveMetaPath(string id);
}