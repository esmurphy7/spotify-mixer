using System.Collections.Generic;
using System.Linq;
using SpotifyMixerApi.Models.Spotify;

namespace SpotifyMixerApi.Models.Transforms
{
    public class OrderTransform : IPlaylistTransform
    {
        public string AttributeName { get; set; }
        public bool Ascending { get; set; } = true;

        public List<SpotifyTrack> Transform(List<SpotifyTrack> tracks)
        {
            if (Ascending)
                return tracks.OrderBy(track => GetTrackAttribute(track, AttributeName)).ToList();
            else
                return tracks.OrderByDescending(track => GetTrackAttribute(track, AttributeName)).ToList();
        }

        private object GetTrackAttribute(SpotifyTrack track, string attributeName)
        {
            return attributeName.ToLower() switch
            {
                "popularity" => track.Popularity,
                "duration_ms" => track.Duration_Ms,
                "name" => track.Name,
                "artist" => track.Artists.FirstOrDefault()?.Name ?? "",
                "album" => track.Album.Name,
                "release_date" => track.Album.Release_Date,
                _ => ""
            };
        }
    }
} 