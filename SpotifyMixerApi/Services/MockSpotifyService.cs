using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public class MockSpotifyService : ISpotifyService
    {
        public Task<List<SpotifyTrack>> GetPlaylistTracksAsync(string playlistId)
        {
            // Return mock data for testing
            var tracks = new List<SpotifyTrack>
            {
                new SpotifyTrack { Id = "1", Name = "Mock Track 1", Popularity = 75, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } },
                new SpotifyTrack { Id = "2", Name = "Mock Track 2", Popularity = 85, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } },
                new SpotifyTrack { Id = "3", Name = "Mock Track 3", Popularity = 65, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } }, Album = new SpotifyAlbum { Name = "Album 3" } },
                new SpotifyTrack { Id = "4", Name = "Mock Track 4", Popularity = 90, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 4" } }, Album = new SpotifyAlbum { Name = "Album 4" } },
                new SpotifyTrack { Id = "5", Name = "Mock Track 5", Popularity = 70, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 5" } }, Album = new SpotifyAlbum { Name = "Album 5" } }
            };

            return Task.FromResult(tracks);
        }

        public Task<string> CreatePlaylistAsync(string name, string userId)
        {
            // Return a mock playlist ID
            return Task.FromResult($"mock-playlist-{Guid.NewGuid()}");
        }

        public Task AddTracksToPlaylistAsync(string playlistId, List<SpotifyTrack> tracks)
        {
            // Mock implementation - in a real service, this would call Spotify API
            return Task.CompletedTask;
        }
    }
} 