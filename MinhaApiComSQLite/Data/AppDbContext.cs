using Microsoft.EntityFrameworkCore;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define que o Nome do Produto deve ser único no banco
            modelBuilder.Entity<Produto>()
                .HasIndex(p => p.Nome)
                .IsUnique();

            // Define que o Nome da Categoria deve ser único no banco
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nome)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}