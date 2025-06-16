using System;
using BookStoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookStoreMVC.Data;

public class ApplicationDbContext : DbContext
{
    // Removed incorrect DeleteBehaviour declaration

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(r => r.Author)
            .WithMany(b => b.Books)
            .HasForeignKey(r => r.AuthorID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .Property(r => r.Price)
            .HasColumnType("decimal(8,2)");

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                AuthorID = 001,
                Name = "Andrea Hirata",
                Bio = "Penulis Novel Inspiratif",
            },

            new Author
            {
                AuthorID = 002,
                Name = "J.K. Rowling",
                Bio = "Penulis Novel Fiksi",
            },

            new Author
            {
                AuthorID = 003,
                Name = "Pramoedya Ananta Toer",
                Bio = "Penulis Novel Sejarah",
            },

            new Author
            {
                AuthorID = 004,
                Name = "J.R.R. Tolkien",
                Bio = "Penulis Novel Fantasi",
            },

            new Author
            {
                AuthorID = 005,
                Name = "J.R.R. Tolkien",
                Bio = "Penulis Novel Fantasi",
            }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                BookID = 001,
                AuthorID = 001,
                Title = "Laskar Pelangi",
                Price = 10.91M,
                ReleaseDate = new DateTime(2005 - 03 - 01)
            },

            new Book
            {
                BookID = 002,
                AuthorID = 002,
                Title = "Harry Potter and The Philosopher's Stone",
                Price = 30.45M,
                ReleaseDate = new DateTime(1997 - 06 - 26)
            },

            new Book
            {
                BookID = 003,
                AuthorID = 003,
                Title = "Bumi Manusia",
                Price = 25.10M,
                ReleaseDate = new DateTime(1980 - 02 - 23)
            },

            new Book
            {
                BookID = 004,
                AuthorID = 004,
                Title = "The Lord Of The Ring The Fellowship Of The Ring",
                Price = 50.67M,
                ReleaseDate = new DateTime(1954 - 07 - 29)
            },

            new Book
            {
                BookID = 005,
                AuthorID = 005,
                Title = "The Lord Of The Ring The Two Towers",
                Price = 40.43M,
                ReleaseDate = new DateTime(2009 - 10 - 06)
            }

        );
    }
}
