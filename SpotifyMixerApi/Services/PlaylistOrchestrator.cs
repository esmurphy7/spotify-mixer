using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistOrchestrator
    {
        Task<List<Track>> MixPlaylistAsync(string playlistId, Mixer mixer);
    }

    public class PlaylistOrchestrator : IPlaylistOrchestrator
    {
        private readonly IPlaylistProvider _playlistProvider;
        private readonly IPlaylistMixer _playlistMixer;

        public PlaylistOrchestrator(IPlaylistProvider playlistProvider, IPlaylistMixer playlistMixer)
        {
            _playlistProvider = playlistProvider;
            _playlistMixer = playlistMixer;
        }

        public async Task<List<Track>> MixPlaylistAsync(string playlistId, Mixer mixer)
        {
            // 1. Fetch tracks from Spotify playlist
            var tracks = await _playlistProvider.GetPlaylistTracksAsync(playlistId);
            
            // 2. Apply mixer transforms
            var mixedTracks = _playlistMixer.MixPlaylist(tracks, mixer);
            
            return mixedTracks;
        }
    }
} 