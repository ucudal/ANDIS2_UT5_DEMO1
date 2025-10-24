// ANDIS2.Monolith.Application/Products/Models/CreateProductRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Monolith.Application.Products.Models;

public class CreateProductRequest
{
    [Required]
    public string Sku { get; set; } = string.Empty;

    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 999999)]
    public decimal Price { get; set; }
}
