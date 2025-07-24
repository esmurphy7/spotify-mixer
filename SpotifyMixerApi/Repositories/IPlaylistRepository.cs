using System.Threading.Tasks;
using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Repositories
{
    public interface IPlaylistRepository
    {
        Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId);
    }
} 