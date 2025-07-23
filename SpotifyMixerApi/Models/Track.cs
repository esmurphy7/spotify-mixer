using System.Collections.Generic;

namespace SpotifyMixerApi.Models
{
    public class Track
    {
        public string id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
} 