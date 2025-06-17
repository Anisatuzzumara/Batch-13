using LibraryManagementMVC.Models;
using LibraryManager.Infrastructure.Repositories;

namespace LibraryManagementMVC.Infrastructure.Repositories
{
    public interface IPublisherRepository : IGenericRepository<Publisher>
    {
        Task<IEnumerable<Publisher>> GetTopPublishersAsync(int count);
    }
}