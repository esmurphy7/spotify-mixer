using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public interface IRepository<TId, T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(TId id);
        Task AddAsync(TId id, T item);
        Task UpdateAsync(TId id, T item);
        Task DeleteAsync(TId id);
    }
} 