using OrderNotificationApi.Models;

namespace OrderNotificationApi.DTOs;

/// <summary>
/// DTO for returning notification data in API responses.
/// </summary>
public class NotificationResponse
{
    public Guid Id { get; set; }
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// Maps a Notification entity to a NotificationResponse DTO.
    /// </summary>
    public static NotificationResponse FromNotification(OrderNotificationApi.Models.Notification notification)
    {
        return new NotificationResponse
        {
            Id = notification.Id,
            OrderId = notification.OrderId,
            OrderNumber = notification.OrderNumber,
            RecipientEmail = notification.RecipientEmail,
            Type = notification.Type.ToString(),
            Subject = notification.Subject,
            Message = notification.Message,
            Status = notification.Status.ToString(),
            CreatedAt = notification.CreatedAt,
            SentAt = notification.SentAt
        };
    }
}
