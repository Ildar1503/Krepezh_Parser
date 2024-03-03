using KrepParser.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace KrepParser.Infrastruction.DataBase
{
    public sealed class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) 
            :base(options) => Database.EnsureCreated();

        public DbSet<Product> Products => Set<Product>(); 

        //TODO: по ходу расширения функционала работы с бд добавить настройки конфиг в криетор.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            var product = Product.CreateProduct(Guid.NewGuid(), "гвозди", 12, "левша");

            // Add initial data to User and Record entities
            modelBuilder.Entity<Product>().HasData(product);
        }
    }
}
