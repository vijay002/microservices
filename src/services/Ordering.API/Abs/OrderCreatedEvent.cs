namespace Ordering.API.Abs
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
    }
}
