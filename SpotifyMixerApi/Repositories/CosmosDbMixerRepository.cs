using Microsoft.Azure.Cosmos;
using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class CosmosDbMixerRepository : IMixerRepository
    {
        private readonly Container _container;

        public CosmosDbMixerRepository(Container container)
        {
            _container = container;
        }

        public async Task<List<Mixer>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Mixer>("SELECT * FROM c");
            var results = new List<Mixer>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<Mixer?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Mixer>(id.ToString(), new PartitionKey(id.ToString()));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddAsync(Mixer mixer)
        {
            await _container.CreateItemAsync(mixer, new PartitionKey(mixer.Id.ToString()));
        }

        public async Task UpdateAsync(Mixer mixer)
        {
            await _container.UpsertItemAsync(mixer, new PartitionKey(mixer.Id.ToString()));
        }

        public async Task DeleteAsync(int id)
        {
            await _container.DeleteItemAsync<Mixer>(id.ToString(), new PartitionKey(id.ToString()));
        }
    }
} 