using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce_microservico.microservico_estoque.Messaging.Events.RabbitMq
{
    public interface IRabbitMqService
    {
        void StartConsuming();

    }
}