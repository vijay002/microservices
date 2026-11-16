namespace Notification.API.BackGroundService;

// <summary>
/// Configuration settings for Azure Service Bus connectivity.
/// </summary>
public class AzureServiceBusSettings
{
    public const string SectionName = "AzureServiceBus";

    /// <summary>
    /// The connection string for the Azure Service Bus namespace.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// The name of the queue to listen for order notification messages.
    /// </summary>
    public string QueueName { get; set; } = "order-notifications";

    /// <summary>
    /// Maximum number of concurrent calls to the message handler.
    /// </summary>
    public int MaxConcurrentCalls { get; set; } = 5;

    /// <summary>
    /// Whether to auto-complete messages after processing.
    /// When false, the service must explicitly complete or abandon messages.
    /// </summary>
    public bool AutoCompleteMessages { get; set; } = false;
}
