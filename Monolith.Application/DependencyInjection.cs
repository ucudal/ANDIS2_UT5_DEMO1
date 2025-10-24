using Monolith.Application.Orders;
using Monolith.Application.Products;
using Monolith.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Monolith.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IOrdersService, OrdersService>();
        return services;
    }
}
