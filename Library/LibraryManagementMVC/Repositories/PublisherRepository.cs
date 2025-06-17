using LibraryManagementMVC.Data;
using LibraryManagementMVC.Infrastructure.Repositories;
using LibraryManager.Infrastructure.Repositories;
using LibraryManagementMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementMVC.Repository
{
    /// <summary>
    /// Implementasi dari IPublisherRepository.
    /// Mewarisi semua fungsionalitas dari GenericRepository.
    /// </summary>
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Kita bisa meng-override metode jika perlu perilaku khusus,
        // misalnya untuk selalu menyertakan data relasi (eager loading).
        public override async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _context.Publishers.Include(p => p.Books).ToListAsync();
        }

        public override async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.PubId == id);
        }

        public async Task<IEnumerable<Publisher>> GetTopPublishersAsync(int count)
        {
            return await _context.Publishers
                .Include(p => p.Books)
                .OrderByDescending(p => p.Books.Count)
                .Take(count)
                .ToListAsync();
        }
    }
}
