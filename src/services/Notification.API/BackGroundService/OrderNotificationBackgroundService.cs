using Microsoft.Extensions.Options;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using OrderNotificationApi.Models;
using OrderNotificationApi.Services;
namespace Notification.API.BackGroundService
{
    public class OrderNotificationBackgroundService : BackgroundService
    {
        private readonly AzureServiceBusSettings _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderNotificationBackgroundService> _logger;

        private ServiceBusClient? _client;
        private ServiceBusProcessor? _processor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public OrderNotificationBackgroundService(IOptions<AzureServiceBusSettings> settings, IServiceProvider serviceProvider, ILogger<OrderNotificationBackgroundService> logger)
        {
            _settings = settings.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Order Notification Background Service is starting. Queue: {QueueName}", _settings.QueueName);

            try
            {
                _client = new ServiceBusClient(_settings.ConnectionString);

                _processor = _client.CreateProcessor(_settings.QueueName, new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = _settings.MaxConcurrentCalls,
                    AutoCompleteMessages = _settings.AutoCompleteMessages
                });

                _processor.ProcessMessageAsync += HandleMessageAsync;
                _processor.ProcessErrorAsync += HandleErrorAsync;

                await _processor.StartProcessingAsync(stoppingToken);

                _logger.LogInformation("Service Bus processor started successfully. Listening for messages...");

                // Keep the service running until cancellation is requested
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Order Notification Background Service is stopping gracefully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in Order Notification Background Service.");
                throw;
            }
        }

        /// <summary>
        /// Handles an incoming message from the Service Bus queue.
        /// </summary>
        private async Task HandleMessageAsync(ProcessMessageEventArgs args)
        {
            var messageId = args.Message.MessageId;
            var body = args.Message.Body.ToString();

            _logger.LogInformation("Received message {MessageId} from queue '{QueueName}'. Body: {Body}", messageId, _settings.QueueName, body);

            try
            {
                var message = JsonSerializer.Deserialize<OrderNotificationMessage>(body, JsonOptions);

                if (message is null)
                {
                    _logger.LogWarning("Message {MessageId} could not be deserialized. Dead-lettering.", messageId);
                    await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Message body could not be deserialized to OrderNotificationMessage.");
                    return;
                }

                // Use a scope to resolve scoped/transient services safely from a singleton background service
                using var scope = _serviceProvider.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IOrderNotificationProcessor>();

                await processor.ProcessMessageAsync(message);

                // Explicitly complete the message after successful processing
                await args.CompleteMessageAsync(args.Message);

                _logger.LogInformation("Message {MessageId} processed and completed successfully.", messageId);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize message {MessageId}. Dead-lettering.", messageId);
                await args.DeadLetterMessageAsync(args.Message, "InvalidJson",
                    $"JSON deserialization failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message {MessageId}. Abandoning for retry.", messageId);
                await args.AbandonMessageAsync(args.Message);
            }
        }

        /// <summary>
        /// Handles errors from the Service Bus processor.
        /// </summary>
        private Task HandleErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
                "Service Bus error. Source: {ErrorSource}, Namespace: {Namespace}, Entity: {EntityPath}",
                args.ErrorSource, args.FullyQualifiedNamespace, args.EntityPath);

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Notification Background Service is shutting down...");

            if (_processor is not null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            if (_client is not null)
            {
                await _client.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);

            _logger.LogInformation("Order Notification Background Service stopped.");
        }
    }
}
