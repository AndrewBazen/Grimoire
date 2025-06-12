// ---------- Domain/Ports/INotificationSink.cs ----------
namespace Grimoire.Domain.Ports;

/// <summary>
/// A sink for notifications.
/// This is a simple interface that allows for the display of notifications.
/// </summary>
public interface INotificationSink
{
    Task ShowToastAsync(string title, string message, TimeSpan? duration = null);
}
