using System.Collections.Concurrent;
using OrderNotificationApi.DTOs;
using OrderNotificationApi.Models;

namespace OrderNotificationApi.Services;

/// <summary>
/// In-memory implementation of <see cref="IOrderService"/>.
/// Replace with a database-backed implementation for production use.
/// </summary>
public class OrderService : IOrderService
{
    private readonly ConcurrentDictionary<int, Order> _orders = new();
    private readonly INotificationService _notificationService;
    private readonly ILogger<OrderService> _logger;
    private int _orderCounter = 0;

    public OrderService(INotificationService notificationService, ILogger<OrderService> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var orderNumber = $"ORD-{Interlocked.Increment(ref _orderCounter):D6}";

        var order = new Order
        {
            OrderNumber = orderNumber,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            Status = OrderStatus.Pending
        };

        order.TotalAmount = order.Items.Sum(i => i.Subtotal);

        _orders[order.Id] = order;
        _logger.LogInformation("Order {OrderNumber} created for {CustomerName}", orderNumber, request.CustomerName);

        // Send order creation notification
        await _notificationService.SendNotificationAsync(order, NotificationType.OrderCreated);

        return OrderResponse.FromOrder(order);
    }

    public Task<OrderResponse?> GetOrderByIdAsync(int id)
    {
        _orders.TryGetValue(id, out var order);
        return Task.FromResult(order is not null ? OrderResponse.FromOrder(order) : null);
    }

    public Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = _orders.Values
            .OrderByDescending(o => o.CreatedAt)
            .Select(OrderResponse.FromOrder);

        return Task.FromResult(orders);
    }

    public async Task<OrderResponse?> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request)
    {
        if (!_orders.TryGetValue(id, out var order))
            return null;

        var oldStatus = order.Status;
        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        _logger.LogInformation("Order {OrderNumber} status changed from {OldStatus} to {NewStatus}",
            order.OrderNumber, oldStatus, request.Status);

        // Determine notification type based on new status
        var notificationType = request.Status switch
        {
            OrderStatus.Confirmed => NotificationType.OrderConfirmed,
            OrderStatus.Shipped => NotificationType.OrderShipped,
            OrderStatus.Delivered => NotificationType.OrderDelivered,
            OrderStatus.Cancelled => NotificationType.OrderCancelled,
            _ => (NotificationType?)null
        };

        if (notificationType.HasValue)
        {
            await _notificationService.SendNotificationAsync(order, notificationType.Value);
        }

        return OrderResponse.FromOrder(order);
    }

    public Task<bool> DeleteOrderAsync(int id)
    {
        var removed = _orders.TryRemove(id, out _);
        return Task.FromResult(removed);
    }
}
