using Grimoire.Domain.Models;

namespace Grimoire.Components.UI;

public partial class NotificationWindow : ContentPage
{
    public NotificationWindow()
    {
        InitializeComponent();
    }

    public NotificationWindow(Notification notification) : this()
    {
        MessageLabel.Text = notification.Message;
        
        // Set background color based on notification type using theme colors
        if (Application.Current != null) {
            switch (notification.Type)
            {
            case Notification.NotificationType.Success:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Success"];
                break;
            case Notification.NotificationType.Error:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Error"];
                break;
            case Notification.NotificationType.Warning:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Warning"];
                break;
            case Notification.NotificationType.Info:
            default:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Info"];
                break;
            
            }
        }
    }

    private void OnOkButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    // Method to close the notification
    public void Close()
    {
        Navigation.PopModalAsync();
    }
} 