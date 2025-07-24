using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistProvider
    {
        Task<List<SpotifyTrack>> GetPlaylistTracksAsync(string playlistId);
    }
} 