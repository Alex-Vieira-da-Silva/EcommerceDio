using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ecommerce_microservico.Data;
using ecommerce_microservico.Entities;
using ecommerce_microservico.Services;

namespace ecommerce_microservico.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly EstoqueDbContext _context;

        public ProdutoService(EstoqueDbContext context)
            => _context = context;

        public async Task<IEnumerable<Produto>> GetAllAsync(CancellationToken ct = default)
            => await _context.Produtos
                             .AsNoTracking()
                             .ToListAsync(ct);

        public async Task<Produto?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Produtos
                             .AsNoTracking()
                             .FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task<Produto> CreateAsync(Produto produto, CancellationToken ct = default)
        {
            produto.Id = Guid.NewGuid();
            await _context.Produtos.AddAsync(produto, ct);
            await _context.SaveChangesAsync(ct);
            return produto;
        }

        public async Task<bool> UpdateAsync(Produto produto, CancellationToken ct = default)
        {
            var existente = await _context.Produtos.FindAsync(new object[] { produto.Id }, ct);
            if (existente is null) 
                return false;

            existente.Nome = produto.Nome;
            existente.Quantidade = produto.Quantidade;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existente = await _context.Produtos.FindAsync(new object[] { id }, ct);
            if (existente is null) 
                return false;

            _context.Produtos.Remove(existente);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}