using LibraryManagementMVC.Models;
using LibraryManagementMVC.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementMVC.Data;

namespace LibraryManagementMVC.Repository
{
    /// <summary>
    /// Implementasi dari IBookRepository.
    /// Mewarisi semua fungsionalitas dari GenericRepository.
    /// </summary>
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Override untuk selalu menyertakan data Publisher saat mengambil data buku.
        public override async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b => b.Publisher).ToListAsync();
        }

        public Task<IEnumerable<Book>> GetBooksByAuthorAsync(string authorName)
        {
            throw new NotImplementedException();
        }

        public override async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.Publisher).FirstOrDefaultAsync(b => b.PubId == id);
        }
    }
}