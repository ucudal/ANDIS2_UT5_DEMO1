using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Orders.Models;

public class CreateOrderRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required, MinLength(1)]
    public List<CreateOrderItem> Items { get; set; } = new();
}

public class CreateOrderItem
{
    [Required]
    public Guid ProductId { get; set; }

    [Range(1, 100000)]
    public int Quantity { get; set; }

    [Range(0, 999999)]
    public decimal UnitPrice { get; set; }
}
