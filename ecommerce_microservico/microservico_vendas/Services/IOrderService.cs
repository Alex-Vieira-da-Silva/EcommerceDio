using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ecommerce_microservico.microservico_vendas.Entities;
using ecommerce_microservico.microservico_vendas.Entities.DTOs;

namespace ecommerce_microservico.microservico_vendas.Services
{
    /// <summary>
    /// Contrato para operações de gerenciamento de pedidos.
    /// </summary>
    public interface IOrderService
    {
        Task<Order> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> CancelAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}