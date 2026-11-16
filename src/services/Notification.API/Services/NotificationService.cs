using System.Collections.Concurrent;
using NotificationApi.Services;
using OrderNotificationApi.Models;
using OrderNotificationApi.Services;

namespace NotificationApi.Services;

/// <summary>
/// In-memory implementation of <see cref="INotificationService"/>.
/// Simulates sending notifications (e.g., email). Replace with a real
/// notification provider (SendGrid, AWS SES, etc.) for production use.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ConcurrentDictionary<Guid, OrderNotificationApi.Models.Notification> _notifications = new();
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public Task<OrderNotificationApi.Models.Notification> SendNotificationAsync(Order order, NotificationType type)
    {
        var (subject, message) = GenerateContent(order, type);

        var notification = new OrderNotificationApi.Models.Notification
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            RecipientEmail = order.CustomerEmail,
            Type = type,
            Subject = subject,
            Message = message,
            Status = NotificationStatus.Sent,
            SentAt = DateTime.UtcNow
        };

        _notifications[notification.Id] = notification;

        _logger.LogInformation(
            "Notification [{Type}] sent to {Email} for order {OrderNumber}: {Subject}",
            type, order.CustomerEmail, order.OrderNumber, subject);

        return Task.FromResult(notification);
    }

    public Task<IEnumerable<OrderNotificationApi.Models.Notification>> GetNotificationsByOrderIdAsync(int orderId)
    {
        var notifications = _notifications.Values
            .Where(n => n.OrderId == orderId)
            .OrderByDescending(n => n.CreatedAt)
            .AsEnumerable();

        return Task.FromResult(notifications);
    }

    public Task<IEnumerable<OrderNotificationApi.Models.Notification>> GetAllNotificationsAsync()
    {
        var notifications = _notifications.Values
            .OrderByDescending(n => n.CreatedAt)
            .AsEnumerable();

        return Task.FromResult(notifications);
    }

    public Task<OrderNotificationApi.Models.Notification?> GetNotificationByIdAsync(Guid id)
    {
        _notifications.TryGetValue(id, out var notification);
        return Task.FromResult(notification);
    }

    /// <summary>
    /// Generates subject and body content for a notification based on order and type.
    /// </summary>
    private static (string Subject, string Message) GenerateContent(Order order, NotificationType type)
    {
        return type switch
        {
            NotificationType.OrderCreated => (
                $"Order {order.OrderNumber} – Confirmation",
                $"Dear {order.CustomerName},\n\nThank you for your order! Your order {order.OrderNumber} has been received and is being processed.\n\nOrder Total: {order.TotalAmount:C}\nItems: {order.Items.Count}\n\nWe will notify you when your order status changes.\n\nBest regards,\nOrder Notification System"
            ),
            NotificationType.OrderConfirmed => (
                $"Order {order.OrderNumber} – Confirmed",
                $"Dear {order.CustomerName},\n\nGreat news! Your order {order.OrderNumber} has been confirmed and is being prepared for shipment.\n\nBest regards,\nOrder Notification System"
            ),
            NotificationType.OrderShipped => (
                $"Order {order.OrderNumber} – Shipped",
                $"Dear {order.CustomerName},\n\nYour order {order.OrderNumber} has been shipped! You can expect delivery soon.\n\nBest regards,\nOrder Notification System"
            ),
            NotificationType.OrderDelivered => (
                $"Order {order.OrderNumber} – Delivered",
                $"Dear {order.CustomerName},\n\nYour order {order.OrderNumber} has been delivered. We hope you enjoy your purchase!\n\nBest regards,\nOrder Notification System"
            ),
            NotificationType.OrderCancelled => (
                $"Order {order.OrderNumber} – Cancelled",
                $"Dear {order.CustomerName},\n\nYour order {order.OrderNumber} has been cancelled. If you did not request this, please contact our support team.\n\nBest regards,\nOrder Notification System"
            ),
            _ => (
                $"Order {order.OrderNumber} – Update",
                $"Dear {order.CustomerName},\n\nThere has been an update to your order {order.OrderNumber}.\n\nBest regards,\nOrder Notification System"
            )
        };
    }
}
