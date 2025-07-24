using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpotifyMixerApi.Models
{
    public class Mixer
    {
        public string id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SrcPlaylistId { get; set; } = string.Empty;
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public List<IPlaylistTransform> Transforms { get; set; } = new();
    }
} 