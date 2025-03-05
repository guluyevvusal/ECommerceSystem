using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using OrderService.models;
using OrderService.Service;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RabbitMQProducer _rabbitMQProducer;

    public OrderController(AppDbContext context, RabbitMQProducer rabbitMQProducer)
    {
        _context = context;
        _rabbitMQProducer = rabbitMQProducer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var orderMessage = JsonSerializer.Serialize(order);
        _rabbitMQProducer.SendOrderCreatedMessageAsync(orderMessage);

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        return order;
    }
}
