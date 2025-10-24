using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Products.Models;

public class UpdateProductRequest
{
    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Price { get; set; }
}
