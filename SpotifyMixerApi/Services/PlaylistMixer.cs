using SpotifyMixerApi.Models;
using System.Collections.Generic;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistMixer
    {
        List<SpotifyTrack> MixPlaylist(List<SpotifyTrack> tracks, Mixer mixer);
    }

    public class PlaylistMixer : IPlaylistMixer
    {
        public List<SpotifyTrack> MixPlaylist(List<SpotifyTrack> tracks, Mixer mixer)
        {
            if (tracks == null)
                throw new ArgumentNullException(nameof(tracks));
            if (mixer == null)
                throw new ArgumentNullException(nameof(mixer));
            if (mixer.Transforms == null || mixer.Transforms.Count == 0)
                return new List<SpotifyTrack>(tracks);

            var mixedTracks = new List<SpotifyTrack>(tracks);
            foreach (var transform in mixer.Transforms)
            {
                if (transform != null)
                    mixedTracks = transform.Transform(mixedTracks);
            }
            return mixedTracks;
        }
    }
} 