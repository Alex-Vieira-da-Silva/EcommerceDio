namespace microservico_vendas.Messaging.Events
{
    public record OrderItemDto(Guid ProductId, int Quantity);

    public record OrderCreatedEvent(Guid OrderId, IEnumerable<OrderItemDto> Items);
}