// ---------- Domain/Ports/INotificationRepository.cs ----------
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Ports;

/// <summary>
/// A repository for notifications.
/// This is a simple interface that allows for the loading and saving of notifications.
/// </summary>
public interface INotificationRepository
{
    Task<IReadOnlyList<Notification>> LoadAsync(CancellationToken ct = default);
    Task SaveAsync(Notification note, CancellationToken ct = default);
}