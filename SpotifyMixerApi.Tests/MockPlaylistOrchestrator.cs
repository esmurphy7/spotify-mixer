using SpotifyMixerApi.Models;
using SpotifyMixerApi.Services;

namespace SpotifyMixerApi.Tests
{
    public class MockPlaylistOrchestrator : IPlaylistOrchestrator
    {
        private readonly List<Track> _mockTracks;

        public MockPlaylistOrchestrator()
        {
            _mockTracks = new List<Track>
            {
                new Track { id = "1", Name = "Mock Track 1", Attributes = new Dictionary<string, object> { { "genre", "rock" }, { "popularity", 75 } } },
                new Track { id = "2", Name = "Mock Track 2", Attributes = new Dictionary<string, object> { { "genre", "pop" }, { "popularity", 85 } } },
                new Track { id = "3", Name = "Mock Track 3", Attributes = new Dictionary<string, object> { { "genre", "jazz" }, { "popularity", 65 } } }
            };
        }

        public Task<List<Track>> MixPlaylistAsync(string playlistId, Mixer mixer)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentException("Playlist ID cannot be null or empty");
            }

            if (mixer == null)
            {
                throw new ArgumentNullException(nameof(mixer));
            }

            // Apply the mixer's transforms to the mock tracks
            var mixedTracks = new List<Track>(_mockTracks);
            foreach (var transform in mixer.Transforms)
            {
                if (transform != null)
                {
                    mixedTracks = transform.Transform(mixedTracks);
                }
            }

            return Task.FromResult(mixedTracks);
        }
    }
} 