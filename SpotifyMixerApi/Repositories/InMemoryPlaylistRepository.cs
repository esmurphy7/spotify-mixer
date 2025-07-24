using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Repositories
{
    public class InMemoryPlaylistRepository : IPlaylistRepository
    {
        private readonly Dictionary<string, List<Track>> _playlists = new();

        public Task<List<Track>> GetPlaylistTracksAsync(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty", nameof(playlistId));
            }

            if (_playlists.TryGetValue(playlistId, out var tracks))
            {
                return Task.FromResult(new List<Track>(tracks));
            }

            // Return empty list for non-existent playlists
            return Task.FromResult(new List<Track>());
        }

        // Helper method for tests to add custom playlists
        public void AddPlaylist(string playlistId, List<Track> tracks)
        {
            _playlists[playlistId] = new List<Track>(tracks);
        }
    }
} 