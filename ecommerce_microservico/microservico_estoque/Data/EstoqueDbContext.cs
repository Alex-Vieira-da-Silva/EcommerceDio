using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ecommerce_microservico.microservico_estoque.Entities;

namespace ecommerce_microservico.Data
{
    public class EstoqueDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public EstoqueDbContext(
            DbContextOptions<EstoqueDbContext> options,
            IConfiguration configuration
        ) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connStr = _configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(connStr);
            }
        }
    }
}