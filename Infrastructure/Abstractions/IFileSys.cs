// ---------- Infrastructure/Abstractions/IFileSystem.cs ----------
// Simple abstraction to replace static System.IO calls in unit tests.
namespace Grimoire.Infrastructure.Abstractions;

public interface IFileSys
{
    string[] GetFiles(string directory, string searchPattern);
    Task WriteAllTextAsync(string path, string contents, CancellationToken ct = default);
    Task<string> ReadAllTextAsync(string path, CancellationToken ct = default);
    bool FileExists(string path);
}
