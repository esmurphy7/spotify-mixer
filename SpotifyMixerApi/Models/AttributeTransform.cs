using System.Collections.Generic;
using System.Linq;

namespace SpotifyMixerApi.Models
{
    public class AttributeTransform : IPlaylistTransform
    {
        public string AttributeName { get; set; }
        public object AttributeValue { get; set; }

        public List<Track> Transform(List<Track> tracks)
        {
            return tracks.Where(t =>
                t.Attributes != null &&
                t.Attributes.TryGetValue(AttributeName, out var value) &&
                Equals(value, AttributeValue)
            ).ToList();
        }
    }
} 