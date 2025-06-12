using System.IO;
using Grimoire.Infrastructure.Abstractions;
using Microsoft.Maui.Storage;

namespace Grimoire.Infrastructure.Implementations;

/// <summary>
/// Provides a writable root path within the application's private data directory.
/// </summary>
public sealed class AppPathProvider : IPathProvider
{
    public string ArchiveRoot { get; }

    public AppPathProvider()
    {
        // FileSystem.AppDataDirectory is per-platform and writable.
        ArchiveRoot = Path.Combine(FileSystem.AppDataDirectory, "archive");
        if (!Directory.Exists(ArchiveRoot))
        {
            Directory.CreateDirectory(ArchiveRoot);
        }
    }

    public string ArchiveMetaPath(string id) => Path.Combine(ArchiveRoot, $"{id}.json");
    public string TomePath(string id) => Path.Combine(ArchiveRoot, $"{id}.json");
    public string EntryPath(string id) => Path.Combine(ArchiveRoot, $"{id}.json");
} 