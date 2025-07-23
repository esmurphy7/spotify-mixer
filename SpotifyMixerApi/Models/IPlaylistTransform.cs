using System.Collections.Generic;

namespace SpotifyMixerApi.Models
{
    public interface IPlaylistTransform
    {
        List<Track> Transform(List<Track> tracks);
    }
} 