using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace Ordering.API.Abs
{
    public class ServiceBusPublisher(ServiceBusClient serviceBusClient) : IServiceBusPublisher
    {
        private const string QueueName = "orders";
        private const string TopicName = "order-events";
        public async Task PublishOrderCreatedEventAsync(OrderCreatedEvent orderCreatedEvent)
        {
            var messagebody = JsonSerializer.Serialize(orderCreatedEvent);

            var message = new ServiceBusMessage(messagebody)
            {
                MessageId = Convert.ToString(orderCreatedEvent.OrderId),
                Subject = orderCreatedEvent.Status,
                ApplicationProperties =
                {
                    { "CustomerOrderId" , orderCreatedEvent.OrderId },
                    { "TotalAmount", orderCreatedEvent.Amount }
                }
            };

            //Publish to Queue
            await using var queueSender = serviceBusClient.CreateSender(QueueName);
            await queueSender.SendMessageAsync(message);

            //publish to Topic
            //await using var topicSender = serviceBusClient.CreateSender(TopicName);
            //await topicSender.SendMessageAsync(message);

        }
    }
}
