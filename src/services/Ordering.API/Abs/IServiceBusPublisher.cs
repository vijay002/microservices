namespace Ordering.API.Abs
{
    public interface IServiceBusPublisher
    {
        public Task PublishOrderCreatedEventAsync(OrderCreatedEvent orderCreatedEvent);
    }
}
