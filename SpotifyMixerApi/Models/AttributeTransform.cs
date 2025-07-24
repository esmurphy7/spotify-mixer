using System.Collections.Generic;
using System.Linq;

namespace SpotifyMixerApi.Models
{
    public class AttributeTransform : IPlaylistTransform
    {
        public string AttributeName { get; set; }
        public object AttributeValue { get; set; }

        public List<SpotifyTrack> Transform(List<SpotifyTrack> tracks)
        {
            return tracks.Where(track =>
                GetTrackAttribute(track, AttributeName)?.Equals(AttributeValue) == true
            ).ToList();
        }

        private object GetTrackAttribute(SpotifyTrack track, string attributeName)
        {
            return attributeName.ToLower() switch
            {
                "popularity" => track.Popularity,
                "duration_ms" => track.Duration_Ms,
                "explicit" => track.Explicit,
                "artist" => track.Artists.FirstOrDefault()?.Name,
                "album" => track.Album.Name,
                "release_date" => track.Album.Release_Date,
                _ => null
            };
        }
    }
} 