using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public interface ISpotifyService
    {
        Task<List<Track>> GetPlaylistTracksAsync(string playlistId);
        Task<string> CreatePlaylistAsync(string name, string userId);
        Task AddTracksToPlaylistAsync(string playlistId, List<Track> tracks);
    }
} 