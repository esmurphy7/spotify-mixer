using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class InMemoryRepository<TId, T> : IRepository<TId, T> where T : class
    {
        private readonly ConcurrentDictionary<TId, T> _items = new();

        public Task<List<T>> GetAllAsync() => Task.FromResult(_items.Values.ToList());

        public Task<T?> GetAsync(TId id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task AddAsync(TId id, T item)
        {
            _items[id] = item;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TId id, T item)
        {
            _items[id] = item;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TId id)
        {
            _items.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
} 