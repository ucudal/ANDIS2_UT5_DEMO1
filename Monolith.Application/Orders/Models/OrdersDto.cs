namespace Monolith.Application.Orders.Models;

public record OrderDto(Guid Id, Guid UserId, string Status, List<OrderItemDto> Items);

public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
