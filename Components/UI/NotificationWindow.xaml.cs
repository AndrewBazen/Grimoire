using Grimoire.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

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
            case NotificationType.Success:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Success"];
                break;
            case NotificationType.Error:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Error"];
                break;
            case NotificationType.Warning:
                NotificationFrame.BackgroundColor = (Color)Application.Current.Resources["Warning"];
                break;
            case NotificationType.Info:
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