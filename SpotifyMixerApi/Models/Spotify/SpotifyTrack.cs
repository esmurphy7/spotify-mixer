using System.Collections.Generic;

namespace SpotifyMixerApi.Models.Spotify
{
    public class SpotifyTrack
    {
        public SpotifyAlbum Album { get; set; }
        public List<SpotifyArtist> Artists { get; set; }
        public List<string> Available_Markets { get; set; }
        public int Disc_Number { get; set; }
        public int Duration_Ms { get; set; }
        public bool Explicit { get; set; }
        public SpotifyExternalIds External_Ids { get; set; }
        public SpotifyExternalUrls External_Urls { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public bool Is_Playable { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public string Preview_Url { get; set; }
        public int Track_Number { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public bool Is_Local { get; set; }
    }
} 