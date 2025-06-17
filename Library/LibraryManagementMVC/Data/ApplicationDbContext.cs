using Microsoft.EntityFrameworkCore;
using LibraryManagementMVC;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(p => p.PubId);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                
                entity.HasMany(p => p.Books)                 
                      .WithOne(b => b.Publisher)              
                      .HasForeignKey(b => b.PubId)      
                      .OnDelete(DeleteBehavior.Restrict);     
            });
            
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BookId);

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(b => b.Author)
                    .IsRequired()
                    .HasMaxLength(150);
                
                entity.HasIndex(b => b.ISBN)
                    .IsUnique();
            });

            
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PubId = 1, Name = "Gramedia Pustaka Utama", Country = "Indonesia" },
                new Publisher { PubId = 2, Name = "Mizan Pustaka", Country = "Indonesia" },
                new Publisher { PubId = 3, Name = "Penguin Books", Country = "United Kingdom" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Laskar Pelangi", Author = "Andrea Hirata", ISBN = "978-979-3062-79-2", PublishedYear = 2005, PubId = 2 },
                new Book { BookId = 2, Title = "Bumi Manusia", Author = "Pramoedya Ananta Toer", ISBN = "978-979-97312-3-4", PublishedYear = 1980, PubId = 1 },
                new Book { BookId = 3, Title = "1984", Author = "George Orwell", ISBN = "978-0-452-28423-4", PublishedYear = 1949, PubId = 3 }
            );
        }
    }
} 

