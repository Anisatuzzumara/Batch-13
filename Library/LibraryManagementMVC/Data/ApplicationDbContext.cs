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

                entity.Property(p => p.Country)
                    .HasMaxLength(100);

                entity.Property(p => p.City)
                    .HasMaxLength(100);

                entity.Property(p => p.Email)
                    .HasMaxLength(100);

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
                new Publisher { PubId = 1, Name = "Gramedia Pustaka Utama", Country = "Indonesia", City = "Bandung", Email = "Gramediapustaka@gmail.com" },
                new Publisher { PubId = 2, Name = "KPG (Kepustakaan Populer Gramedia)", Country = "Indonesia", City = "Jakarta", Email = "KPG14@gmail.com"  },
                new Publisher { PubId = 3, Name = "Delacorte Press", Country = "United Kingdom", City = "London", Email = "DelacortePress@gmail.com" }

            );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Laskar Pelangi", Author = "Andrea Hirata", ISBN = "978-979-3062-79-2", PublishedYear = 2005, PubId = 2 },
                new Book { BookId = 2, Title = "Bumi Manusia", Author = "Pramoedya Ananta Toer", ISBN = "978-979-97312-3-4", PublishedYear = 1980, PubId = 1 },
                new Book { BookId = 3, Title = "The Maze Runner", Author = "James Dashner", ISBN = "978-979-433-655-7", PublishedYear = 2009, PubId = 3 },
                new Book { BookId = 4, Title = "Laut Bercerita", Author = "Leila S. Chudori", ISBN ="978-979-91-0644-1", PublishedYear = 2010, PubId=1}
            );
        }
    }
} 

