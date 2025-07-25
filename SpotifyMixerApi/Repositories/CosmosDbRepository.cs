using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class CosmosDbRepository<T> : IRepository<T> where T : class
    {
        private readonly Container _container;
        private static string GetId(T item)
        {
            var prop = typeof(T).GetProperty("id");
            return prop?.GetValue(item)?.ToString() ?? throw new System.Exception("Object must have an 'id' property");
        }

        public CosmosDbRepository(Container container)
        {
            _container = container;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<T>("SELECT * FROM c");
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddAsync(T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(GetId(item)));
        }

        public async Task UpdateAsync(T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(GetId(item)));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }
    }
} 