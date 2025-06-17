using LibraryManagementMVC.Models;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManagementMVC.Infrastructure.Repositories
{
    /// <summary>
    /// Interface repository khusus untuk entitas Publisher.
    /// Mewarisi semua metode dari IGenericRepository.
    /// </summary>
    public interface IPublisherRepository : IGenericRepository<Publisher>
    {
        // Di masa depan, metode khusus untuk Publisher bisa ditambahkan di sini.
        Task<IEnumerable<Publisher>> GetTopPublishersAsync(int count);
    }
}