using Monolith.Application.Orders;
using Monolith.Application.Orders.Models;
using Microsoft.AspNetCore.Mvc;

namespace ANDIS2.Monolith.Web.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _service;
    public OrdersController(IOrdersService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        var order = await _service.GetByIdAsync(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderDto>> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
    {
        var updated = await _service.UpdateStatusAsync(id, request.Status);
        return updated is null ? NotFound() : Ok(updated);
    }
    
    [HttpGet("user/{userId}/details")]
    public async Task<ActionResult<List<OrderDetailsForUserDto>>> GetDetailsForUser(Guid userId)
    {
        var details = await _service.GetDetailsForUserAsync(userId);
        if (details.Count == 0) return NotFound();
        return Ok(details);
    }
}
