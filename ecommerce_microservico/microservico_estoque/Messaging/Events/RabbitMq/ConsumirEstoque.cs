using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_microservico.microservico_estoque.Messaging.Events.RabbitMq
{
    public class ConsumirEstoque
    {
        private readonly IModel _channel;

        public StockConsumer(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:User"],
                Password = config["RabbitMQ:Pass"]
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.ExchangeDeclare("orders-exchange", ExchangeType.Direct, durable: true);
            var queue = _channel.QueueDeclare(queue: "order-created-queue", durable: true).QueueName;
            _channel.QueueBind(queue, "orders-exchange", "order.created");
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var orderCreated = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                // aqui debitar estoque, disparar outros fluxos etc.
            };
            _channel.BasicConsume(queue: "order-created-queue",
                                  autoAck: true,
                                  consumer: consumer);
        }
    }

}