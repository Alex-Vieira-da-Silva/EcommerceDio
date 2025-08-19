using Microsoft.EntityFrameworkCore;
using ecommerce_microservico.microservico_vendas.Entities;

namespace ecommerce_microservico.microservico_vendas.Data
{
    public class ContextDbVendas : DbContext
    {
        public ContextDbVendas(DbContextOptions<ContextDbVendas> options)
            : base(options)
        {
        }

        /// <summary>
        /// Conjunto de pedidos.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Conjunto de itens de pedido.
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
                entity.HasMany(o => o.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Quantidade)
                      .IsRequired();
                entity.Property(i => i.UnitPrice)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}