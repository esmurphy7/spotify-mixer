using Xunit;
using SpotifyMixerApi.Services;
using SpotifyMixerApi.Models.Transforms;
using SpotifyMixerApi.Models.Spotify;
using System.Collections.Generic;
using System.Linq;

public class PlaylistMixerTests
{
    private List<SpotifyTrack> SampleTracks => new()
    {
        new SpotifyTrack { Id = "1", Name = "Track1", Popularity = 10, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } },
        new SpotifyTrack { Id = "2", Name = "Track2", Popularity = 20, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } },
        new SpotifyTrack { Id = "3", Name = "Track3", Popularity = 30, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 3" } },
        new SpotifyTrack { Id = "4", Name = "Track4", Popularity = 40, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } }, Album = new SpotifyAlbum { Name = "Album 4" } },
    };

    public static IEnumerable<object[]> MixerTestData()
    {
        yield return new object[]
        {
            new List<IPlaylistTransform> { new TakeTransform { Count = 2, FromStart = true } },
            new[] { "1", "2" }
        };
        yield return new object[]
        {
            new List<IPlaylistTransform> { new AttributeTransform { AttributeName = "artist", AttributeValue = "Artist 1" } },
            new[] { "1", "3" }
        };
        yield return new object[]
        {
            new List<IPlaylistTransform> { new OrderTransform { AttributeName = "popularity", Ascending = false } },
            new[] { "4", "3", "2", "1" }
        };
    }

    /// <summary>
    /// Tests MixPlaylist with various transforms and checks the resulting track IDs.
    /// </summary>
    /// <param name="transforms">A list of playlist transforms to apply (e.g., TakeTransform, AttributeTransform, OrderTransform).</param>
    /// <param name="expectedIds">The expected track IDs in the resulting playlist after transforms are applied.</param>
    [Theory]
    [MemberData(nameof(MixerTestData))]
    public void MixPlaylist_Theory(List<IPlaylistTransform> transforms, string[] expectedIds)
    {
        var mixer = new PlaylistMixer();
        var result = mixer.MixPlaylist(SampleTracks, new SpotifyMixerApi.Models.Mixer { Transforms = transforms });
        Assert.Equal(expectedIds, result.Select(t => t.Id));
    }
} 