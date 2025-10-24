namespace Monolith.Application.Orders.Models;

public class OrderDetailsForUserDto
{
    public Guid OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
}
