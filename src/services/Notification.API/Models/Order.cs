namespace OrderNotificationApi.Models;

/// <summary>
/// Represents an order in the system.
/// </summary>
public class Order
{
    //public Guid Id { get; set; } = Guid.NewGuid();
    public int Id { get; set; } = 0;
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
