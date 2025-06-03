using Grimoire.Models;
using Grimoire.Components.UI;

namespace Grimoire.Services;

public class NotificationService
{
    public NotificationService()
    {
    }

    public async Task ShowNotificationAsync(string message, NotificationType type = NotificationType.Info)
    {
        if (message == null) return;
        await ShowNotificationAsync(new Notification(message, type));
    }
    
    public async Task ShowNotificationAsync(Notification notification)
    {
        if (notification == null) return;
        
        var notificationWindow = new NotificationWindow(notification);
        
        if (Application.Current?.Windows.Count > 0 && Application.Current?.Windows[0] != null)
        {
            var mainWindow = Application.Current.Windows[0];
            if (mainWindow.Page != null)
            {
                await mainWindow.Page.Navigation.PushModalAsync(notificationWindow);
            } else {
                await DisplayAlert("Notification", notification.Message, "OK");
            }
        }
        else
        {
            // Fallback for when no windows are available
            await DisplayAlert("Notification", notification.Message, "OK");
        }
    }
    
    private async Task DisplayAlert(string title, string message, string button)
    {
        if (title == null || message == null || button == null) return;
        if (Application.Current?.Windows.Count > 0)
        {
            var mainWindow = Application.Current.Windows[0];
            if (mainWindow.Page != null)
            {
                await mainWindow.Page.DisplayAlert(title, message, button);
            }
        }
    }
}