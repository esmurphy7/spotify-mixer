using System.Collections.Generic;

namespace SpotifyMixerApi.Models
{
    public interface IPlaylistTransform
    {
        List<SpotifyTrack> Transform(List<SpotifyTrack> tracks);
    }
} 