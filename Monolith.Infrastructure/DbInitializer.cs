using Monolith.Domain.Users;
using Monolith.Domain.Products;
using Monolith.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Monolith.Infrastructure;

public static class DbInitializer
{
    // Ejecuta migraciones y si la DB está vacía, agrega datos de prueba
    public static async Task InitializeAsync(AppDbContext db)
    {
        // Aplica migraciones pendientes, creando la DB si no existe
        //** await db.Database.MigrateAsync();
        
        if (await db.Users.AnyAsync() || await db.Products.AnyAsync() || await db.Orders.AnyAsync())
            return;

        // Usuarios de prueba
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Email = "alice@example.com", Name = "Alice", CreatedAt = DateTime.UtcNow },
            new User { Id = Guid.NewGuid(), Email = "bob@example.com",   Name = "Bob",   CreatedAt = DateTime.UtcNow },
        };
        db.Users.AddRange(users);

        // Productos de prueba
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Sku = "SKU-1001", Name = "Camisa", Price = 1200.50m, CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Sku = "SKU-1002", Name = "Pantalón", Price = 2300.00m, CreatedAt = DateTime.UtcNow },
            new Product { Id = Guid.NewGuid(), Sku = "SKU-1003", Name = "Zapatos", Price = 3500.75m, CreatedAt = DateTime.UtcNow }
        };
        db.Products.AddRange(products);

        // Órdenes de prueba (usa los primeros user y productos)
        var demoUser = users[0];
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = demoUser.Id,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = products[0].Id, Quantity = 2, UnitPrice = products[0].Price },
                new OrderItem { ProductId = products[1].Id, Quantity = 1, UnitPrice = products[1].Price },
            }
        };
        db.Orders.Add(order);
        
        demoUser = users[1];
        order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = demoUser.Id,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = products[2].Id, Quantity = 5, UnitPrice = products[0].Price },
            }
        };
        db.Orders.Add(order);

        await db.SaveChangesAsync();
    }
}
