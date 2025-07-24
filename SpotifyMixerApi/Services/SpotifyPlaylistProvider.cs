using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public class SpotifyPlaylistProvider : IPlaylistProvider
    {
        private readonly ISpotifyService _spotifyService;

        public SpotifyPlaylistProvider(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        public async Task<List<SpotifyTrack>> GetPlaylistTracksAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));
            }

            return await _spotifyService.GetPlaylistTracksAsync(playlistId);
        }
    }
} 