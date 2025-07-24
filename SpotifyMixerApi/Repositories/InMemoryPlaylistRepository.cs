using SpotifyMixerApi.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Repositories
{
    public class InMemoryPlaylistRepository : IPlaylistRepository
    {
        private readonly ConcurrentDictionary<string, SpotifyPlaylist> _playlists = new();

        public Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));

            if (_playlists.TryGetValue(playlistId, out var playlist))
                return Task.FromResult(playlist);

            // Return an empty playlist if not found
            return Task.FromResult<SpotifyPlaylist>(null);
        }

        // Helper for tests
        public void AddPlaylist(string playlistId, SpotifyPlaylist playlist)
        {
            _playlists[playlistId] = playlist;
        }
    }
} 