using Monolith.Application.Products.Models;
using Monolith.Domain.Products;
using Monolith.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Monolith.Application.Products;

public class ProductsService : IProductsService
{
    private readonly AppDbContext _db;
    public ProductsService(AppDbContext db) => _db = db;

    public async Task<List<ProductDto>> GetAllAsync() =>
        await _db.Products.AsNoTracking()
            .Select(p => new ProductDto(p.Id, p.Sku, p.Name, p.Price))
            .ToListAsync();

    public async Task<ProductDto?> GetByIdAsync(Guid id) =>
        await _db.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductDto(p.Id, p.Sku, p.Name, p.Price))
            .FirstOrDefaultAsync();

    public async Task<ProductDto> CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = request.Sku.Trim(),
            Name = request.Name.Trim(),
            Price = request.Price,
            CreatedAt = DateTime.UtcNow
        };
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return new ProductDto(product.Id, product.Sku, product.Name, product.Price);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return null;
        product.Name = request.Name.Trim();
        product.Price = request.Price;
        await _db.SaveChangesAsync();
        return new ProductDto(product.Id, product.Sku, product.Name, product.Price);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product is null) return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}
