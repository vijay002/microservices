using OrderNotificationApi.Models;

namespace OrderNotificationApi.Services;

/// <summary>
/// Interface for processing order notification messages received from Azure Service Bus.
/// </summary>
public interface IOrderNotificationProcessor
{
    /// <summary>
    /// Processes an incoming order notification message.
    /// </summary>
    /// <param name="message">The deserialized order notification message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessMessageAsync(OrderNotificationMessage message);
}
