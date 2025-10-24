using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Orders.Models;

public class UpdateOrderStatusRequest
{
    [Required]
    public string Status { get; set; } = "Pending";
}
