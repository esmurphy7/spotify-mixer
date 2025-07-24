using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public interface ISpotifyService
    {
        Task<SpotifyPlaylist> GetPlaylistAsync(string playlistId);
        Task<string> CreatePlaylistAsync(string name, string userId);
        Task AddTracksToPlaylistAsync(string playlistId, List<SpotifyTrack> tracks);
    }
} 