using System.Threading.Tasks;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;

namespace SpotifyMixerApi.Repositories
{
    public interface IPlaylistRepository
    {
        Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId);
    }
} 