using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistOrchestrator
    {
        Task<SpotifyPlaylist> MixPlaylistAsync(string playlistId, Mixer mixer);
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

        public async Task<SpotifyPlaylist> MixPlaylistAsync(string playlistId, Mixer mixer)
        {
            var playlist = await _playlistRepository.GetPlaylistAsync(playlistId);
            if (playlist == null || playlist.Tracks == null)
                return null;

            var originalTracks = playlist.Tracks.Items.Select(i => i.Track).ToList();
            var mixedTracks = _playlistMixer.MixPlaylist(originalTracks, mixer);

            // Return a new playlist object with mixed tracks
            var mixedPlaylist = new SpotifyPlaylist
            {
                Collaborative = playlist.Collaborative,
                Description = playlist.Description,
                External_Urls = playlist.External_Urls,
                Href = playlist.Href,
                Id = playlist.Id,
                Images = playlist.Images,
                Name = playlist.Name + " (Mixed)",
                Owner = playlist.Owner,
                Public = playlist.Public,
                Snapshot_Id = playlist.Snapshot_Id,
                Type = playlist.Type,
                Uri = playlist.Uri,
                Tracks = new SpotifyPlaylistTracks
                {
                    Href = playlist.Tracks.Href,
                    Limit = playlist.Tracks.Limit,
                    Next = playlist.Tracks.Next,
                    Offset = playlist.Tracks.Offset,
                    Previous = playlist.Tracks.Previous,
                    Total = mixedTracks.Count,
                    Items = mixedTracks.Select(t => new SpotifyPlaylistTrackItem { Track = t }).ToList()
                }
            };
            return mixedPlaylist;
        }
    }
} 