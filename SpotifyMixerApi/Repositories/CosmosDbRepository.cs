using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class CosmosDbRepository<TId, T> : IRepository<TId, T> where T : class
    {
        private readonly Container _container;

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

        public async Task<T?> GetAsync(TId id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id?.ToString(), new PartitionKey(id?.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddAsync(TId id, T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(id?.ToString()));
        }

        public async Task UpdateAsync(TId id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id?.ToString()));
        }

        public async Task DeleteAsync(TId id)
        {
            await _container.DeleteItemAsync<T>(id?.ToString(), new PartitionKey(id?.ToString()));
        }
    }
} 