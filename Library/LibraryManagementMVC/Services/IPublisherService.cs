using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task<IEnumerable<Publisher>> SearchPublishersAsync(string searchTerm);
        Task CreatePublisherAsync(Publisher publisher);
        Task UpdatePublisherAsync(Publisher publisher);
        Task<bool> DeletePublisherAsync(int id);
    }
}
