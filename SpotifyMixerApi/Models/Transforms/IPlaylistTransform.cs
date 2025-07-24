using System.Collections.Generic;
using SpotifyMixerApi.Models.Spotify;

namespace SpotifyMixerApi.Models.Transforms
{
    public interface IPlaylistTransform
    {
        List<SpotifyTrack> Transform(List<SpotifyTrack> tracks);
    }
} 