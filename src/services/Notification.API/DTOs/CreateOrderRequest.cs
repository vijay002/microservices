using System.ComponentModel.DataAnnotations;

namespace OrderNotificationApi.DTOs;

/// <summary>
/// DTO for creating a new order.
/// </summary>
public class CreateOrderRequest
{
    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

/// <summary>
/// DTO for an order line item in a create request.
/// </summary>
public class CreateOrderItemRequest
{
    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0.")]
    public decimal UnitPrice { get; set; }
}
