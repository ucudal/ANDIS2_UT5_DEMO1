using Monolith.Application.Orders.Models;
using Monolith.Domain.Orders;
using Monolith.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Monolith.Domain.Products;

namespace Monolith.Application.Orders;

public class OrdersService : IOrdersService
{
    private readonly AppDbContext _db;
    public OrdersService(AppDbContext db) => _db = db;

    public async Task<List<OrderDto>> GetAllAsync() =>
        await _db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .Select(o => new OrderDto(
                o.Id, o.UserId, o.Status,
                o.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
            ))
            .ToListAsync();

    public async Task<OrderDto?> GetByIdAsync(Guid id) =>
        await _db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.Id == id)
            .Select(o => new OrderDto(
                o.Id, o.UserId, o.Status,
                o.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
            ))
            .FirstOrDefaultAsync();

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new OrderDto(
            order.Id, order.UserId, order.Status,
            order.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
        );
    }

    public async Task<OrderDto?> UpdateStatusAsync(Guid id, string status)
    {
        var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) return null;
        order.Status = status;
        await _db.SaveChangesAsync();
        return new OrderDto(
            order.Id, order.UserId, order.Status,
            order.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
        );
    }
    
    public async Task<List<OrderDetailsForUserDto>> GetDetailsForUserAsync(Guid userId)
    {
        // Cargar nombre del usuario (si existe)
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        var customerName = user?.Name;

        // Cargar órdenes del usuario, con items
        var orders = await _db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        if (orders.Count == 0)
            return new List<OrderDetailsForUserDto>();

        var result = new List<OrderDetailsForUserDto>();

        // Pre-cargar productos usados en estas órdenes para evitar N+1
        var allProductIds = orders.SelectMany(o => o.Items.Select(i => i.ProductId)).Distinct().ToList();
        var productNames = await _db.Products.AsNoTracking()
            .Where(p => allProductIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Name);

        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                productNames.TryGetValue(item.ProductId, out var productName);

                result.Add(new OrderDetailsForUserDto
                {
                    OrderId = order.Id,
                    CustomerName = customerName,
                    ProductName = productName,
                    Quantity = item.Quantity
                });
            }
        }

        return result;
    }
}
