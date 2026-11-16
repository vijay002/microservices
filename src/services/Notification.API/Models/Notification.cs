namespace OrderNotificationApi.Models;

/// <summary>
/// Represents a notification sent for an order event.
/// </summary>
public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
}
