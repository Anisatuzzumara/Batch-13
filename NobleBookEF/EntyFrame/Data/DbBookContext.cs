
using System;
using EntyFrame.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace EntyFrame.Data
{
    public class DbBookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=NobleBook");
                optionsBuilder.EnableSensitiveDataLogging();
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(config.GetConnectionString("DBConnection"));
                
                /* Raw, insecure database connection string:
                optionsBuilder.UseNpgsql(
                    $"Host=..." +
                    $"Database=..." +
                    $"Username=..." +
                    $"Password=..."
                );
                */
            }
        }
    }
}


