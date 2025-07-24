using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;
using SpotifyMixerApi.Repositories;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public class SpotifyPlaylistProvider : IPlaylistProvider
    {
        private readonly IPlaylistRepository _playlistRepository;

        public SpotifyPlaylistProvider(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));
            }

            return await _playlistRepository.GetPlaylistAsync(playlistId);
        }
    }
} 