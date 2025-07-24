using Xunit;
using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Linq;
using SpotifyMixerApi.Models.Transforms;
using SpotifyMixerApi.Models.Spotify;

public class TransformTests
{
    private readonly List<SpotifyTrack> SampleTracks = new()
    {
        new SpotifyTrack { Id = "1", Name = "Track1", Popularity = 10, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } },
        new SpotifyTrack { Id = "2", Name = "Track2", Popularity = 20, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } },
        new SpotifyTrack { Id = "3", Name = "Track3", Popularity = 30, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 3" } },
        new SpotifyTrack { Id = "4", Name = "Track4", Popularity = 40, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } }, Album = new SpotifyAlbum { Name = "Album 4" } },
    };

    [Fact]
    public void TakeTransform_TakesFirstNTracks()
    {
        var transform = new TakeTransform { Count = 2, FromStart = true };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].Id);
        Assert.Equal("2", result[1].Id);
    }

    [Fact]
    public void TakeTransform_TakesLastNTracks()
    {
        var transform = new TakeTransform { Count = 2, FromStart = false };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.Equal("3", result[0].Id);
        Assert.Equal("4", result[1].Id);
    }

    [Fact]
    public void AttributeTransform_FiltersByAttribute()
    {
        var transform = new AttributeTransform { AttributeName = "artist", AttributeValue = "Artist 1" };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal("Artist 1", t.Artists.First().Name));
    }

    [Fact]
    public void AttributeTransform_ReturnsEmptyIfNoMatch()
    {
        var transform = new AttributeTransform { AttributeName = "artist", AttributeValue = "Nonexistent" };
        var result = transform.Transform(SampleTracks);
        Assert.Empty(result);
    }

    [Fact]
    public void OrderTransform_OrdersByAttribute_Ascending()
    {
        var transform = new OrderTransform { AttributeName = "popularity", Ascending = true };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(new[] { "1", "2", "3", "4" }, result.Select(t => t.Id));
    }

    [Fact]
    public void OrderTransform_OrdersByAttribute_Descending()
    {
        var transform = new OrderTransform { AttributeName = "popularity", Ascending = false };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(new[] { "4", "3", "2", "1" }, result.Select(t => t.Id));
    }
} 