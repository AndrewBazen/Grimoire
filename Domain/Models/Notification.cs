using CommunityToolkit.Mvvm.ComponentModel;

namespace Grimoire.Domain.Models;

public partial class Notification : ObservableObject
{
    [ObservableProperty]
    private DateTime _timestamp;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private NotificationType _type;

    public Notification()
    {
        Message = string.Empty;
        Type = NotificationType.Info;
        Timestamp = DateTime.UtcNow;
    }

    public Notification(string message, NotificationType type) : this()
    {
        Message = message;
        Type = type;
        Timestamp = DateTime.UtcNow;
    }

    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }
}