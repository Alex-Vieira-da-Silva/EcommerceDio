using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_microservico.microservico_estoque.Messaging.Events.RabbitMq
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration _configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void StartConsuming()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                UserName = _configuration["RabbitMQ:User"],
                Password = _configuration["RabbitMQ:Pass"]
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "estoque-queue", durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Mensagem recebida: {message}");

                // Aqui você pode desserializar e processar a mensagem
                // var objeto = JsonSerializer.Deserialize<SeuTipo>(message);
            };

            channel.BasicConsume(queue: "estoque-queue", autoAck: true, consumer: consumer);
        }
    }
  
}