using SpotifyMixerApi.Models;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistProvider
    {
        Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId);
    }
} 