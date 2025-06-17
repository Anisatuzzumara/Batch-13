using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManager.Infrastructure.Repositories
{
    /// <summary>
    /// Interface generik yang mendefinisikan operasi CRUD dasar untuk semua repository.
    /// </summary>
    /// <typeparam name="T">Tipe entitas yang akan dikelola oleh repository.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
    }
}
