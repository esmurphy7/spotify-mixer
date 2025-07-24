using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Repositories
{
    public interface IPlaylistRepository
    {
        Task<List<Track>> GetPlaylistTracksAsync(string playlistId);
    }
} 