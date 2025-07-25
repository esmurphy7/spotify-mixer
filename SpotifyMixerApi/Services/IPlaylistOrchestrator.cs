using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistOrchestrator
    {
        Task<SpotifyPlaylist> MixPlaylistAsync(string playlistId, Mixer mixer);
    }
} 