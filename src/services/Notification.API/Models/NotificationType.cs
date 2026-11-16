namespace OrderNotificationApi.Models;

/// <summary>
/// Represents the type of notification triggered by an order event.
/// </summary>
public enum NotificationType
{
    OrderCreated = 0,
    OrderConfirmed = 1,
    OrderShipped = 2,
    OrderDelivered = 3,
    OrderCancelled = 4
}
