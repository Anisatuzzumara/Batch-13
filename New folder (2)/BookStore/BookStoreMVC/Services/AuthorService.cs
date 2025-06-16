using System;
using BookStoreMVC.Data;
using BookStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMVC.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync();
    Task<Author?> GetAuthorByIdAsync(int id);
    Task<Author> UpdateAuthorAsync(Author author);
    Task<bool> DeleteAuthorAsync(int id);
    Task<IEnumerable<Author>> SearchAuthorAsync(string searchTerm); // typo diperbaiki
    Task<IEnumerable<Book>> GetBookByAuthorIdAsync(int authorId);
    Task CreateAuthorAsync(Author author);
}

public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext _context;

    public AuthorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors
            .Include(b => b.Books)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<Author> UpdateAuthorAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return false;

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Author?> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors
            .Include(b => b.Books)
            .FirstOrDefaultAsync(b => b.AuthorID == id);
    }

    public async Task<IEnumerable<Author>> SearchAuthorAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAuthorsAsync();

        return await _context.Authors
            .Include(b => b.Books)
            .Where(s => s.Name.Contains(searchTerm) ||
                       (s.Bio != null && s.Bio.Contains(searchTerm)))
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBookByAuthorIdAsync(int authorId)
    {
        return await _context.Books
            .Where(b => b.AuthorID == authorId)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task CreateAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }
}