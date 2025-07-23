using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public interface IMixerRepository
    {
        Task<List<Mixer>> GetAllAsync();
        Task<Mixer?> GetByIdAsync(int id);
        Task AddAsync(Mixer mixer);
        Task UpdateAsync(Mixer mixer);
        Task DeleteAsync(int id);
    }
} 