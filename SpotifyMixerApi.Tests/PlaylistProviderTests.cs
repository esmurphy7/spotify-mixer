using Xunit;
using SpotifyMixerApi.Services;
using SpotifyMixerApi.Repositories;
using SpotifyMixerApi.Models.Spotify;
using System.Threading.Tasks;

public class PlaylistProviderTests
{
    private SpotifyPlaylistProvider GetProviderWithTestData()
    {
        var repo = new InMemoryPlaylistRepository();
        repo.AddPlaylist("playlist-1", new SpotifyPlaylist { Id = "playlist-1", Name = "Test Playlist", Tracks = new SpotifyPlaylistTracks { Items = new System.Collections.Generic.List<SpotifyPlaylistTrackItem>() } });
        return new SpotifyPlaylistProvider(repo);
    }

    [Theory]
    [InlineData("playlist-1", true, false)] // valid id, expect playlist, not exception
    [InlineData(null, false, true)] // null id, expect exception
    [InlineData("", false, true)] // empty id, expect exception
    [InlineData("notfound", false, false)] // missing id, expect null
    public async Task GetPlaylistAsync_Theory(string playlistId, bool expectPlaylist, bool expectException)
    {
        var provider = GetProviderWithTestData();
        if (expectException)
        {
            await Assert.ThrowsAsync<System.ArgumentException>(() => provider.GetPlaylistAsync(playlistId));
        }
        else
        {
            var playlist = await provider.GetPlaylistAsync(playlistId);
            if (expectPlaylist)
            {
                Assert.NotNull(playlist);
                Assert.Equal(playlistId, playlist.Id);
            }
            else
            {
                Assert.Null(playlist);
            }
        }
    }
} 