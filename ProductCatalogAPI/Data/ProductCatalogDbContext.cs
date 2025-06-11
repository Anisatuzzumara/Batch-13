using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Data
{    /// <summary>
    /// Database context for Product Catalog API.
    /// This is the bridge between the C# application and the database.
    /// </summary>
    public class ProductCatalogDbContext : DbContext
    {
        public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options)
        {
        }

        // DbSet represents tables in the database
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Setup relationship between Product and Category
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal precision for Price
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");

                // Indexes for performance
                entity.HasIndex(p => p.CategoryId);
                entity.HasIndex(p => p.IsActive);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                // Unique constraint on category name
                entity.HasIndex(c => c.Name)
                      .IsUnique();

                entity.HasIndex(c => c.IsActive);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Seed Categories
            var categories = new[]
            {
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", CreatedDate = baseDate.AddDays(1), IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Books, magazines, and reading materials", CreatedDate = baseDate.AddDays(2), IsActive = true },
                new Category { Id = 3, Name = "Clothing", Description = "Apparel and fashion items", CreatedDate = baseDate.AddDays(3), IsActive = true },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home improvement and gardening supplies", CreatedDate = baseDate.AddDays(4), IsActive = true }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Seed Products
            var products = new[]
            {
                new Product { Id = 1, Name = "Smartphone X1", Description = "Latest smartphone with advanced features", Price = 699.99m, StockQuantity = 50, CategoryId = 1, CreatedDate = baseDate.AddDays(10), LastModifiedDate = baseDate.AddDays(10), IsActive = true },
                new Product { Id = 2, Name = "Laptop Pro 15", Description = "High-performance laptop for professionals", Price = 1299.99m, StockQuantity = 25, CategoryId = 1, CreatedDate = baseDate.AddDays(11), LastModifiedDate = baseDate.AddDays(11), IsActive = true },
                new Product { Id = 3, Name = "Programming Fundamentals", Description = "Learn the basics of programming", Price = 49.99m, StockQuantity = 100, CategoryId = 2, CreatedDate = baseDate.AddDays(12), LastModifiedDate = baseDate.AddDays(12), IsActive = true },
                new Product { Id = 4, Name = "Web Development Mastery", Description = "Advanced web development techniques", Price = 79.99m, StockQuantity = 75, CategoryId = 2, CreatedDate = baseDate.AddDays(13), LastModifiedDate = baseDate.AddDays(13), IsActive = true },
                new Product { Id = 5, Name = "Cotton T-Shirt", Description = "Comfortable cotton t-shirt", Price = 19.99m, StockQuantity = 200, CategoryId = 3, CreatedDate = baseDate.AddDays(14), LastModifiedDate = baseDate.AddDays(14), IsActive = true },
                new Product { Id = 6, Name = "Garden Tools Set", Description = "Complete set of gardening tools", Price = 89.99m, StockQuantity = 30, CategoryId = 4, CreatedDate = baseDate.AddDays(15), LastModifiedDate = baseDate.AddDays(15), IsActive = true }
            };

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}