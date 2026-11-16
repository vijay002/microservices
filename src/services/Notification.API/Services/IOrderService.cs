using OrderNotificationApi.DTOs;
using OrderNotificationApi.Models;

namespace OrderNotificationApi.Services;

/// <summary>
/// Interface for order management operations.
/// </summary>
public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse?> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request);
    Task<bool> DeleteOrderAsync(int id);
}
