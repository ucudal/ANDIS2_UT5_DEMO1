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
        await db.Database.MigrateAsync();

        if (await db.Users.AnyAsync() || await db.Products.AnyAsync() || await db.Orders.AnyAsync())
            return;

        var now = DateTime.UtcNow;

        // Usuarios de prueba (Ids fijos para poder usarlos en Ucu.Andis.Monolith.Web.http)
        var alice = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "alice@example.com", Name = "Alice", CreatedAt = now.AddDays(-30) };
        var bob   = new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Email = "bob@example.com",   Name = "Bob",   CreatedAt = now.AddDays(-20) };
        var carol = new User { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Email = "carol@example.com", Name = "Carol", CreatedAt = now.AddDays(-10) };
        var dave  = new User { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Email = "dave@example.com",  Name = "Dave",  CreatedAt = now.AddDays(-2) };
        db.Users.AddRange(alice, bob, carol, dave);

        // Productos de prueba
        var camisa   = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001001"), Sku = "SKU-1001", Name = "Camisa",   Price = 1200.50m, CreatedAt = now.AddDays(-60) };
        var pantalon = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001002"), Sku = "SKU-1002", Name = "Pantalón", Price = 2300.00m, CreatedAt = now.AddDays(-60) };
        var zapatos  = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001003"), Sku = "SKU-1003", Name = "Zapatos",  Price = 3500.75m, CreatedAt = now.AddDays(-60) };
        var campera  = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001004"), Sku = "SKU-1004", Name = "Campera",  Price = 4800.00m, CreatedAt = now.AddDays(-45) };
        var medias   = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001005"), Sku = "SKU-1005", Name = "Medias",   Price = 250.00m,  CreatedAt = now.AddDays(-45) };
        var gorro    = new Product { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000001006"), Sku = "SKU-1006", Name = "Gorro",    Price = 650.90m,  CreatedAt = now.AddDays(-15) };
        db.Products.AddRange(camisa, pantalon, zapatos, campera, medias, gorro);

        // Órdenes de prueba en distintos estados; Dave no tiene órdenes
        db.Orders.AddRange(
            NewOrder("bbbbbbbb-0000-0000-0000-000000000001", alice, "Pending", now.AddDays(-1),
                (camisa, 2), (pantalon, 1)),
            NewOrder("bbbbbbbb-0000-0000-0000-000000000002", alice, "Completed", now.AddDays(-25),
                (zapatos, 1), (medias, 3)),
            NewOrder("bbbbbbbb-0000-0000-0000-000000000003", bob, "Completed", now.AddDays(-15),
                (zapatos, 5)),
            NewOrder("bbbbbbbb-0000-0000-0000-000000000004", bob, "Paid", now.AddDays(-3),
                (campera, 1), (gorro, 2)),
            NewOrder("bbbbbbbb-0000-0000-0000-000000000005", carol, "Cancelled", now.AddDays(-8),
                (pantalon, 2)),
            NewOrder("bbbbbbbb-0000-0000-0000-000000000006", carol, "Pending", now,
                (camisa, 1), (medias, 6), (gorro, 1))
        );

        await db.SaveChangesAsync();
    }

    private static Order NewOrder(string id, User user, string status, DateTime createdAt,
        params (Product Product, int Quantity)[] items) =>
        new()
        {
            Id = Guid.Parse(id),
            UserId = user.Id,
            Status = status,
            CreatedAt = createdAt,
            Items = items.Select(i => new OrderItem
            {
                ProductId = i.Product.Id,
                Quantity = i.Quantity,
                UnitPrice = i.Product.Price
            }).ToList()
        };
}
