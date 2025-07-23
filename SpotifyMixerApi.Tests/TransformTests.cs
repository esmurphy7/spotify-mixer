using Xunit;
using SpotifyMixerApi.Models;
using System.Collections.Generic;
using System.Linq;

public class TransformTests
{
    private readonly List<Track> SampleTracks = new()
    {
        new Track { id = "1", Name = "Track1", Attributes = new Dictionary<string, object> { { "genre", "rock" }, { "popularity", 10 } } },
        new Track { id = "2", Name = "Track2", Attributes = new Dictionary<string, object> { { "genre", "pop" }, { "popularity", 20 } } },
        new Track { id = "3", Name = "Track3", Attributes = new Dictionary<string, object> { { "genre", "rock" }, { "popularity", 30 } } },
        new Track { id = "4", Name = "Track4", Attributes = new Dictionary<string, object> { { "genre", "jazz" }, { "popularity", 40 } } },
    };

    [Fact]
    public void TakeTransform_TakesFirstNTracks()
    {
        var transform = new TakeTransform { Count = 2, FromStart = true };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].id);
        Assert.Equal("2", result[1].id);
    }

    [Fact]
    public void TakeTransform_TakesLastNTracks()
    {
        var transform = new TakeTransform { Count = 2, FromStart = false };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.Equal("3", result[0].id);
        Assert.Equal("4", result[1].id);
    }

    [Fact]
    public void AttributeTransform_FiltersByAttribute()
    {
        var transform = new AttributeTransform { AttributeName = "genre", AttributeValue = "rock" };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal("rock", t.Attributes["genre"]));
    }

    [Fact]
    public void AttributeTransform_ReturnsEmptyIfNoMatch()
    {
        var transform = new AttributeTransform { AttributeName = "genre", AttributeValue = "classical" };
        var result = transform.Transform(SampleTracks);
        Assert.Empty(result);
    }

    [Fact]
    public void OrderTransform_OrdersByAttribute_Ascending()
    {
        var transform = new OrderTransform { AttributeName = "popularity", Ascending = true };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(new[] { "1", "2", "3", "4" }, result.Select(t => t.id));
    }

    [Fact]
    public void OrderTransform_OrdersByAttribute_Descending()
    {
        var transform = new OrderTransform { AttributeName = "popularity", Ascending = false };
        var result = transform.Transform(SampleTracks);
        Assert.Equal(new[] { "4", "3", "2", "1" }, result.Select(t => t.id));
    }
} 