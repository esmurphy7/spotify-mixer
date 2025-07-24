using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public class MockSpotifyService : ISpotifyService
    {
        public Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId)
        {
            // Return mock playlist data for testing
            var playlist = new SpotifyPlaylist
            {
                Id = playlistId,
                Name = $"Mock Playlist {playlistId}",
                Tracks = new SpotifyPlaylistTracks
                {
                    Items = new List<SpotifyPlaylistTrackItem>
                    {
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "1", Name = "Mock Track 1", Popularity = 75, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "2", Name = "Mock Track 2", Popularity = 85, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "3", Name = "Mock Track 3", Popularity = 65, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } }, Album = new SpotifyAlbum { Name = "Album 3" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "4", Name = "Mock Track 4", Popularity = 90, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 4" } }, Album = new SpotifyAlbum { Name = "Album 4" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "5", Name = "Mock Track 5", Popularity = 70, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 5" } }, Album = new SpotifyAlbum { Name = "Album 5" } } }
                    }
                }
            };
            return Task.FromResult(playlist);
        }

        public Task<string> CreatePlaylistAsync(string name, string userId)
        {
            // Return a mock playlist ID
            return Task.FromResult($"mock-playlist-{System.Guid.NewGuid()}");
        }

        public Task AddTracksToPlaylistAsync(string playlistId, List<SpotifyTrack> tracks)
        {
            // Mock implementation - in a real service, this would call Spotify API
            return Task.CompletedTask;
        }
    }
} 