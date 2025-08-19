namespace microservico_vendas.Messaging.RabbitMq
{
    public interface IRabbitMqService
    {
        void Publish<T>(T message, string exchange, string routingKey);
    }
}