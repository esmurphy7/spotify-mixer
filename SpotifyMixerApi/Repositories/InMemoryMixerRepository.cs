using SpotifyMixerApi.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class InMemoryMixerRepository : IMixerRepository
    {
        private readonly ConcurrentDictionary<int, Mixer> _mixers = new();

        public Task<List<Mixer>> GetAllAsync() => Task.FromResult(_mixers.Values.ToList());

        public Task<Mixer?> GetByIdAsync(int id)
        {
            _mixers.TryGetValue(id, out var mixer);
            return Task.FromResult(mixer);
        }

        public Task AddAsync(Mixer mixer)
        {
            _mixers[mixer.Id] = mixer;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Mixer mixer)
        {
            _mixers[mixer.Id] = mixer;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            _mixers.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
} 