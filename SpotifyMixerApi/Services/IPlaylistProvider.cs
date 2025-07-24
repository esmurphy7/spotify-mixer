using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistProvider
    {
        Task<List<Track>> GetPlaylistTracksAsync(string playlistId);
    }
} 