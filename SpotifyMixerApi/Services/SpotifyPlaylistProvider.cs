using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;
using SpotifyMixerApi.Repositories;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public class SpotifyPlaylistProvider : IPlaylistProvider
    {
        private readonly IRepository<SpotifyPlaylist> _playlistRepository;

        public SpotifyPlaylistProvider(IRepository<SpotifyPlaylist> playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));
            }

            return await _playlistRepository.GetByIdAsync(playlistId);
        }
    }
} 