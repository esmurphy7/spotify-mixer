using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotifyMixerApi.Models
{
    public class Mixer
    {
        public string id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<IPlaylistTransform> Transforms { get; set; } = new();
    }
} 