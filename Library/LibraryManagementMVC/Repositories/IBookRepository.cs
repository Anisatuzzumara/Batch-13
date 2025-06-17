

using LibraryManagementMVC.Models;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManagementMVC.Repository
{
    /// <summary>
    /// Interface repository khusus untuk entitas Book.
    /// Mewarisi semua metode dari IGenericRepository.
    /// </summary>
    public interface IBookRepository : IGenericRepository<Book>
    {
        // Di masa depan, metode khusus untuk Book bisa ditambahkan di sini.
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string authorName);
    }
}
