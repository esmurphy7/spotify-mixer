using System.Collections.Generic;
using System.Linq;

namespace SpotifyMixerApi.Models
{
    public class OrderTransform : IPlaylistTransform
    {
        public string AttributeName { get; set; }
        public bool Ascending { get; set; } = true;

        public List<Track> Transform(List<Track> tracks)
        {
            if (Ascending)
                return tracks.OrderBy(t => t.Attributes?[AttributeName]).ToList();
            else
                return tracks.OrderByDescending(t => t.Attributes?[AttributeName]).ToList();
        }
    }
} 