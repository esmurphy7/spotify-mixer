using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotifyMixerApi.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(TakeTransform), "TakeTransform")]
    [JsonDerivedType(typeof(AttributeTransform), "AttributeTransform")]
    [JsonDerivedType(typeof(OrderTransform), "OrderTransform")]
    public interface IPlaylistTransform
    {
        List<Track> Transform(List<Track> tracks);
    }
} 