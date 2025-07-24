using System.Collections.Generic;

namespace SpotifyMixerApi.Models
{
    public class SpotifyPlaylist
    {
        public bool Collaborative { get; set; }
        public string Description { get; set; }
        public SpotifyExternalUrls External_Urls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<SpotifyImage> Images { get; set; }
        public string Name { get; set; }
        public SpotifyUser Owner { get; set; }
        public bool? Public { get; set; }
        public string Snapshot_Id { get; set; }
        public SpotifyPlaylistTracks Tracks { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }
} 