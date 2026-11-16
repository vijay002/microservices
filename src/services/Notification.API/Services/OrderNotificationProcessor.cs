using OrderNotificationApi.Models;

namespace OrderNotificationApi.Services;

/// <summary>
/// Processes order notification messages received from Azure Service Bus.
/// Maps incoming messages to internal notification records.
/// </summary>
public class OrderNotificationProcessor : IOrderNotificationProcessor
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<OrderNotificationProcessor> _logger;

    public OrderNotificationProcessor(
        INotificationService notificationService,
        ILogger<OrderNotificationProcessor> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task ProcessMessageAsync(OrderNotificationMessage message)
    {
        _logger.LogInformation(
            "Processing Service Bus message for Order {OrderNumber}, Event: {EventType}",
            message.OrderNumber, message.EventType);

        // Parse the event type from the message
        if (!Enum.TryParse<NotificationType>(message.EventType, ignoreCase: true, out var notificationType))
        {
            _logger.LogWarning(
                "Unknown event type '{EventType}' for order {OrderNumber}. Skipping notification.",
                message.EventType, message.OrderNumber);
            return;
        }

        // Build a lightweight Order object from the message for the notification service
        var order = new Order
        {
            Id = message.OrderId, // int.TryParse(message.OrderId, out var parsedId) ? parsedId : 0,
            OrderNumber = message.OrderNumber,
            CustomerName = message.CustomerName,
            CustomerEmail = message.CustomerEmail,
            TotalAmount = message.TotalAmount
        }; 

        var notification = await _notificationService.SendNotificationAsync(order, notificationType);

        _logger.LogInformation(
            "Notification {NotificationId} created for order {OrderNumber} — Type: {Type}, Status: {Status}",
            notification.Id, message.OrderNumber, notificationType, notification.Status);
    }
}
