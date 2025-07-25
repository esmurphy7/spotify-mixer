using Xunit;
using SpotifyMixerApi.Services;
using SpotifyMixerApi.Models.Spotify;
using SpotifyMixerApi.Models.Transforms;
using SpotifyMixerApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PlaylistOrchestratorTests
{
    private SpotifyPlaylistProvider GetProviderWithTestData()
    {
        var repo = new InMemoryPlaylistRepository();
        repo.AddPlaylist("playlist-1", new SpotifyPlaylist
        {
            Id = "playlist-1",
            Name = "Test Playlist",
            Tracks = new SpotifyPlaylistTracks
            {
                Items = new List<SpotifyPlaylistTrackItem>
                {
                    new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "1", Name = "Track1", Popularity = 10, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } } },
                    new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "2", Name = "Track2", Popularity = 20, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } } }
                }
            }
        });
        repo.AddPlaylist("playlist-2", new SpotifyPlaylist { Id = "playlist-2", Name = "No Tracks", Tracks = null });
        return new SpotifyPlaylistProvider(repo);
    }

    public static IEnumerable<object[]> OrchestratorTestData()
    {
        yield return new object[]
        {
            "playlist-1",
            new List<IPlaylistTransform> { new TakeTransform { Count = 1, FromStart = true } },
            true, // expect result
            "Test Playlist (Mixed)",
            new[] { "1" }
        };
        yield return new object[]
        {
            "notfound",
            new List<IPlaylistTransform>(),
            false, // expect null
            null,
            null
        };
        yield return new object[]
        {
            "playlist-2",
            new List<IPlaylistTransform>(),
            false, // expect null
            null,
            null
        };
    }

    [Theory]
    [MemberData(nameof(OrchestratorTestData))]
    public async Task MixPlaylistAsync_Theory(string playlistId, List<IPlaylistTransform> transforms, bool expectResult, string expectedName, string[] expectedTrackIds)
    {
        var provider = GetProviderWithTestData();
        var mixer = new PlaylistMixer();
        var orchestrator = new PlaylistOrchestrator(provider, mixer);
        var result = await orchestrator.MixPlaylistAsync(playlistId, new SpotifyMixerApi.Models.Mixer { Transforms = transforms });
        if (expectResult)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Name);
            Assert.Equal(expectedTrackIds, result.Tracks.Items.Select(i => i.Track.Id));
        }
        else
        {
            Assert.Null(result);
        }
    }
} 