using System.Collections.Generic;

namespace SpotifyMixerApi.Models.Spotify
{
    public class SpotifyAlbum
    {
        public string Album_Type { get; set; }
        public int Total_Tracks { get; set; }
        public List<string> Available_Markets { get; set; }
        public SpotifyExternalUrls External_Urls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public List<SpotifyImage> Images { get; set; }
        public string Name { get; set; }
        public string Release_Date { get; set; }
        public string Release_Date_Precision { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public List<SpotifyArtist> Artists { get; set; }
    }
} 