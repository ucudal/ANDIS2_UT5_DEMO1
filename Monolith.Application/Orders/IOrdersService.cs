using Monolith.Application.Orders.Models;

namespace Monolith.Application.Orders;

public interface IOrdersService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(Guid id);
    Task<OrderDto> CreateAsync(CreateOrderRequest request);
    Task<OrderDto?> UpdateStatusAsync(Guid id, string status);
    
    Task<List<OrderDetailsForUserDto>> GetDetailsForUserAsync(Guid userId);

}
