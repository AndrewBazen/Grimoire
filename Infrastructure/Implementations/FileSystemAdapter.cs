using System.IO;
using Grimoire.Infrastructure.Abstractions;

namespace Grimoire.Infrastructure.Implementations;

/// <summary>
/// Thin wrapper around <see cref="System.IO.File"/> & related APIs so they can be mocked in tests.
/// </summary>
public sealed class FileSystemAdapter : IFileSys
{
    public string[] GetFiles(string directory, string searchPattern)
        => Directory.GetFiles(directory, searchPattern);

    public Task WriteAllTextAsync(string path, string contents, CancellationToken ct = default)
        => File.WriteAllTextAsync(path, contents, ct);

    public Task<string> ReadAllTextAsync(string path, CancellationToken ct = default)
        => File.ReadAllTextAsync(path, ct);

    public bool FileExists(string path) => File.Exists(path);
} 