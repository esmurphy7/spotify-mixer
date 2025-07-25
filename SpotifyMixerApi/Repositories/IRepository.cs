using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(string id);
    }
} 