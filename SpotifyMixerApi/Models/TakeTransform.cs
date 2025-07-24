using System;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyMixerApi.Models
{
    public class TakeTransform : IPlaylistTransform
    {
        public int Count { get; set; }
        public bool FromStart { get; set; } = true;

        public List<SpotifyTrack> Transform(List<SpotifyTrack> tracks)
        {
            if (FromStart)
                return tracks.Take(Count).ToList();
            else
                return tracks.Skip(Math.Max(0, tracks.Count - Count)).ToList();
        }
    }
} 