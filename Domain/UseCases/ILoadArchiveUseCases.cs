// ===============================
// Grimoire – Refactor Skeleton
// ===============================
// ---------- Domain/UseCases/LoadArchiveUseCase.cs ----------


namespace Grimoire.Domain.UseCases;

public interface ILoadArchiveUseCase
{
    Task<LoadArchiveResponse> ExecuteAsync(LoadArchiveRequest request, CancellationToken ct = default);
}



