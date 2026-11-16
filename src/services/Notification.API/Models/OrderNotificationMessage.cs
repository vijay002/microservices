namespace OrderNotificationApi.Models;

/// <summary>
/// Represents an order notification message received from Azure Service Bus.
/// </summary>
public class OrderNotificationMessage
{
    /// <summary>
    /// The unique order identifier.
    /// </summary>
    public int OrderId { get; set; } = 0;

    /// <summary>
    /// The order number (e.g. ORD-000001).
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// The customer's full name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The customer's email address for sending notifications.
    /// </summary>
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// The event type that triggered this message (e.g., OrderCreated, OrderShipped).
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the order.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Optional additional data or notes attached to the message.
    /// </summary>
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Timestamp when the event occurred.
    /// </summary>
    public DateTime EventTimestamp { get; set; } = DateTime.UtcNow;

    public int Amount { get; set; }
}
