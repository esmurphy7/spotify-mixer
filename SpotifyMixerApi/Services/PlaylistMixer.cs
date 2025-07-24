using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Services
{
    public interface IPlaylistMixer
    {
        List<Track> MixPlaylist(List<Track> tracks, Mixer mixer);
    }

    public class PlaylistMixer : IPlaylistMixer
    {
        public List<Track> MixPlaylist(List<Track> tracks, Mixer mixer)
        {
            if (tracks == null)
            {
                throw new ArgumentNullException(nameof(tracks));
            }

            if (mixer == null)
            {
                throw new ArgumentNullException(nameof(mixer));
            }

            if (mixer.Transforms == null || mixer.Transforms.Count == 0)
            {
                // If no transforms are specified, return the original tracks
                return new List<Track>(tracks);
            }

            // Apply each transform in sequence
            var mixedTracks = new List<Track>(tracks);
            foreach (var transform in mixer.Transforms)
            {
                if (transform != null)
                {
                    mixedTracks = transform.Transform(mixedTracks);
                }
            }

            return mixedTracks;
        }
    }
} 