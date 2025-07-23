using System.Text.Json.Serialization;

namespace SpotifyMixerApi.Models
{
    public class Mixer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
} 