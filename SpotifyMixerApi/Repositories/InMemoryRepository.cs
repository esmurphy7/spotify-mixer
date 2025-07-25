using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly ConcurrentDictionary<string, T> _items = new();
        private static string GetId(T item)
        {
            var prop = typeof(T).GetProperty("id");
            return prop?.GetValue(item)?.ToString() ?? throw new System.Exception("Object must have an 'id' property");
        }

        public Task<List<T>> GetAllAsync() => Task.FromResult(_items.Values.ToList());

        public Task<T?> GetByIdAsync(string id)
        {
            _items.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task AddAsync(T item)
        {
            _items[GetId(item)] = item;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T item)
        {
            _items[GetId(item)] = item;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            _items.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
} 