using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public class SpotifyPlaylistProvider : IPlaylistProvider
    {
        private readonly ISpotifyService _spotifyService;

        public SpotifyPlaylistProvider(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        public async Task<List<Track>> GetPlaylistTracksAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));
            }

            return await _spotifyService.GetPlaylistTracksAsync(playlistId);
        }
    }
} 