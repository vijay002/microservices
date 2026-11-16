using System.ComponentModel.DataAnnotations;
using OrderNotificationApi.Models;

namespace OrderNotificationApi.DTOs;

/// <summary>
/// DTO for updating the status of an order.
/// </summary>
public class UpdateOrderStatusRequest
{
    [Required]
    public OrderStatus Status { get; set; }
}
