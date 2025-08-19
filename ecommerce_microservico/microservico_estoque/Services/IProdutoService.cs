using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ecommerce_microservico.Entities;

namespace ecommerce_microservico.microservico_estoque.Services
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Produto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Produto> CreateAsync(Produto produto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Produto produto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}