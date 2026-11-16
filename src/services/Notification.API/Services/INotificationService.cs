using OrderNotificationApi.Models;

namespace OrderNotificationApi.Services;

/// <summary>
/// Interface for notification operations.
/// </summary>
public interface INotificationService
{
    Task<OrderNotificationApi.Models.Notification> SendNotificationAsync(Order order, NotificationType type);
    Task<IEnumerable<OrderNotificationApi.Models.Notification>> GetNotificationsByOrderIdAsync(int orderId);
    Task<IEnumerable<OrderNotificationApi.Models.Notification>> GetAllNotificationsAsync();
    Task<OrderNotificationApi.Models.Notification?> GetNotificationByIdAsync(Guid id);
}
