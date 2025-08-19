using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecommerce_microservico.microservico_vendas.Entities.DTOs;
using ecommerce_microservico.microservico_vendas.Services;

namespace ecommerce_microservico.microservico_vendas.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly HttpClient _estoqueClient;

        private readonly IRabbitMqService _rabbitMq;
    public OrdersController(IRabbitMqService rabbitMq) =>
        _rabbitMq = rabbitMq;

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        // lógica síncrona de criação de pedido...
        var orderId = Guid.NewGuid();

        // preparar evento
        var eventMessage = new OrderCreatedEvent(orderId, dto.Items);
        _rabbitMq.Publish(eventMessage, 
                          exchange: "orders-exchange", 
                          routingKey: "order.created");

        return Accepted(new { orderId });
    }

        public OrdersController(
            IOrderService orderService,
            IHttpClientFactory httpClientFactory)
        {
            _orderService = orderService;
            _estoqueClient = httpClientFactory.CreateClient("Estoque");
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(
            [FromBody] CreateOrderDto dto,
            CancellationToken cancellationToken = default)
        {
            // Reserva no estoque
            var estoqueResponse = await _estoqueClient
                .PostAsync(
                    $"api/v1/estoque/produtos/{dto.ProductId}/reserva/{dto.Quantidade}",
                    null,
                    cancellationToken
                );

            if (!estoqueResponse.IsSuccessStatusCode)
                return BadRequest("Falha na validação de estoque.");

            // Persiste pedido
            var order = await _orderService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetById(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var order = await _orderService.GetByIdAsync(id, cancellationToken);
            if (order is null)
                return NotFound();

            return Ok(order);
        }
    }
}