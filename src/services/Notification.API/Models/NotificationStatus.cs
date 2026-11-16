namespace OrderNotificationApi.Models;

/// <summary>
/// Represents the delivery status of a notification.
/// </summary>
public enum NotificationStatus
{
    Pending = 0,
    Sent = 1,
    Failed = 2
}
