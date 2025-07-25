using Xunit;
using SpotifyMixerApi.Services;
using SpotifyMixerApi.Repositories;
using SpotifyMixerApi.Models.Spotify;
using System.Threading.Tasks;

public class PlaylistProviderTests
{
    private SpotifyPlaylistProvider GetProviderWithTestData()
    {
        var repo = new InMemoryRepository<SpotifyPlaylist>();
        repo.AddAsync(new SpotifyPlaylist { Id = "playlist-1", Name = "Test Playlist", Tracks = new SpotifyPlaylistTracks { Items = new System.Collections.Generic.List<SpotifyPlaylistTrackItem>() } }).Wait();
        repo.AddAsync(new SpotifyPlaylist
        {
            Id = "playlist-2",
            Name = "Another Playlist",
            Tracks = new SpotifyPlaylistTracks
            {
                Items = new List<SpotifyPlaylistTrackItem>
                {
                    new SpotifyPlaylistTrackItem
                    {
                        Track = new SpotifyTrack
                        {
                            Id = "track-3",
                            Name = "Track 3",
                            Popularity = 30,
                            Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } },
                            Album = new SpotifyAlbum { Name = "Album 3" }
                        }
                    }
                }
            }
        }).Wait();
        return new SpotifyPlaylistProvider(repo);
    }

    /// <summary>
    /// Tests GetPlaylistAsync for various playlist IDs and expected outcomes.
    /// </summary>
    /// <param name="playlistId">The playlist ID to fetch. Can be a valid ID, null, empty, or not found.</param>
    /// <param name="expectPlaylist">True if a playlist is expected to be returned; false otherwise.</param>
    /// <param name="expectException">True if an ArgumentException is expected (for null or empty ID); false otherwise.</param>
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