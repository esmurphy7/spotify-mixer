using System.Collections.Generic;

namespace SpotifyMixerApi.Models.Spotify
{
    public class SpotifyPlaylistTracks
    {
        public string Href { get; set; }
        public int Limit { get; set; }
        public string Next { get; set; }
        public int Offset { get; set; }
        public string Previous { get; set; }
        public int Total { get; set; }
        public List<SpotifyPlaylistTrackItem> Items { get; set; }
    }

    public class SpotifyPlaylistTrackItem
    {
        public string Added_At { get; set; }
        public SpotifyUser Added_By { get; set; }
        public bool Is_Local { get; set; }
        public SpotifyTrack Track { get; set; }
    }
} 