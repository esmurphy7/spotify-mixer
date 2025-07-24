using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistOrchestrator
    {
        Task<List<Track>> MixPlaylistAsync(string playlistId, Mixer mixer);
    }

    public class PlaylistOrchestrator : IPlaylistOrchestrator
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IPlaylistMixer _playlistMixer;

        public PlaylistOrchestrator(IPlaylistRepository playlistRepository, IPlaylistMixer playlistMixer)
        {
            _playlistRepository = playlistRepository;
            _playlistMixer = playlistMixer;
        }

        public async Task<List<Track>> MixPlaylistAsync(string playlistId, Mixer mixer)
        {
            // 1. Fetch tracks from playlist repository
            var tracks = await _playlistRepository.GetPlaylistTracksAsync(playlistId);
            
            // 2. Apply mixer transforms
            var mixedTracks = _playlistMixer.MixPlaylist(tracks, mixer);
            
            return mixedTracks;
        }
    }
} 