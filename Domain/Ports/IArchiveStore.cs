namespace Grimoire.Domain.Ports;

public interface IArchiveStore
{
    Task<IReadOnlyList<IArchive>> LoadAllAsync(CancellationToken ct = default);
    Task<IArchive>               CreateAsync(string name, CancellationToken ct = default);
    Task<IArchive>               LoadAsync(string id, CancellationToken ct = default);
    Task<IArchive>               SaveAsync(IArchive archive, CancellationToken ct = default);
    Task<IArchive>               DeleteAsync(string id, CancellationToken ct = default);
}