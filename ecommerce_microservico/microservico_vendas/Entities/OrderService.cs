using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ecommerce_microservico.microservico_vendas.Data;
using ecommerce_microservico.microservico_vendas.Entities;
using ecommerce_microservico.microservico_vendas.Entities.DTOs;
using ecommerce_microservico.microservico_vendas.Entities.Enums;
using ecommerce_microservico.microservico_vendas.Services;

namespace ecommerce_microservico.microservico_vendas.Services
{
    public class OrderService : IOrderService
    {
        private readonly ContextDbVendas _dbContext;

        public OrderService(ContextDbVendas dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateAsync(
            CreateOrderDto dto,
            CancellationToken cancellationToken = default)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Status    = OrderStatus.Pending,
                Items     = dto.Items?
                              .Select(i => new OrderItem
                              {
                                  Id         = Guid.NewGuid(),
                                  ProdutoId  = i.ProdutoId,
                                  Quantidade = i.Quantidade,
                                  UnitPrice  = i.UnitPrice
                              })
                              .ToList()
                            ?? new List<OrderItem>()
            };

            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task<Order?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CancelAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            var order = await _dbContext.Orders.FindAsync(
                new object[] { orderId }, cancellationToken);
            if (order == null || order.Status == OrderStatus.Cancelled)
                return false;

            order.Status = OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}