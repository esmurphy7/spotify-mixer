using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public class MockSpotifyService : ISpotifyService
    {
        public Task<List<Track>> GetPlaylistTracksAsync(string playlistId)
        {
            // Return mock data for testing
            var tracks = new List<Track>
            {
                new Track { id = "1", Name = "Mock Track 1", Attributes = new Dictionary<string, object> { { "genre", "rock" }, { "popularity", 75 } } },
                new Track { id = "2", Name = "Mock Track 2", Attributes = new Dictionary<string, object> { { "genre", "pop" }, { "popularity", 85 } } },
                new Track { id = "3", Name = "Mock Track 3", Attributes = new Dictionary<string, object> { { "genre", "jazz" }, { "popularity", 65 } } },
                new Track { id = "4", Name = "Mock Track 4", Attributes = new Dictionary<string, object> { { "genre", "rock" }, { "popularity", 90 } } },
                new Track { id = "5", Name = "Mock Track 5", Attributes = new Dictionary<string, object> { { "genre", "pop" }, { "popularity", 70 } } }
            };

            return Task.FromResult(tracks);
        }

        public Task<string> CreatePlaylistAsync(string name, string userId)
        {
            // Return a mock playlist ID
            return Task.FromResult($"mock-playlist-{Guid.NewGuid()}");
        }

        public Task AddTracksToPlaylistAsync(string playlistId, List<Track> tracks)
        {
            // Mock implementation - in a real service, this would call Spotify API
            return Task.CompletedTask;
        }
    }
} 