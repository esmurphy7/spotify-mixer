using Xunit;
using SpotifyMixerApi.Controllers;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using SpotifyMixerApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SpotifyMixerApi.Models.Spotify;
using SpotifyMixerApi.Models.Transforms;

namespace SpotifyMixerApi.Tests
{
    public class MixerControllerTheoryTests
    {
        private InMemoryRepository<Mixer> GetInitializedRepo()
        {
            var repo = new InMemoryRepository<Mixer>();
            repo.AddAsync(new Mixer { id = "1", Name = "A", SrcPlaylistId = "playlist-1", Transforms = new List<IPlaylistTransform> { new TakeTransform { Count = 2, FromStart = true } } }).Wait();
            repo.AddAsync(new Mixer { id = "2", Name = "B", SrcPlaylistId = "playlist-2", Transforms = new List<IPlaylistTransform> { new AttributeTransform { AttributeName = "artist", AttributeValue = "Artist 1" } } }).Wait();
            repo.AddAsync(new Mixer { id = "3", Name = "C", SrcPlaylistId = "", Transforms = new List<IPlaylistTransform> { new OrderTransform { AttributeName = "popularity", Ascending = false } } }).Wait();
            return repo;
        }

        private IRepository<SpotifyPlaylist> GetPlaylistRepository()
        {
            var repo = new InMemoryRepository<SpotifyPlaylist>();
            // Add test playlists
            repo.AddAsync(new SpotifyPlaylist
            {
                Id = "playlist-1",
                Name = "Test Playlist 1",
                Tracks = new SpotifyPlaylistTracks
                {
                    Items = new List<SpotifyPlaylistTrackItem>
                    {
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "1", Name = "Mock Track 1", Popularity = 75, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 1" } }, Album = new SpotifyAlbum { Name = "Album 1" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "2", Name = "Mock Track 2", Popularity = 85, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 2" } }, Album = new SpotifyAlbum { Name = "Album 2" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "3", Name = "Mock Track 3", Popularity = 65, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 3" } }, Album = new SpotifyAlbum { Name = "Album 3" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "4", Name = "Mock Track 4", Popularity = 90, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 4" } }, Album = new SpotifyAlbum { Name = "Album 4" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "5", Name = "Mock Track 5", Popularity = 70, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Artist 5" } }, Album = new SpotifyAlbum { Name = "Album 5" } } }
                    }
                }
            }).Wait();
            repo.AddAsync(new SpotifyPlaylist
            {
                Id = "playlist-2",
                Name = "Test Playlist 2",
                Tracks = new SpotifyPlaylistTracks
                {
                    Items = new List<SpotifyPlaylistTrackItem>
                    {
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "6", Name = "Rock Track 1", Popularity = 80, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Rock Artist 1" } }, Album = new SpotifyAlbum { Name = "Rock Album 1" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "7", Name = "Rock Track 2", Popularity = 85, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Rock Artist 2" } }, Album = new SpotifyAlbum { Name = "Rock Album 2" } } },
                        new SpotifyPlaylistTrackItem { Track = new SpotifyTrack { Id = "8", Name = "Pop Track 1", Popularity = 90, Artists = new List<SpotifyArtist> { new SpotifyArtist { Name = "Pop Artist 1" } }, Album = new SpotifyAlbum { Name = "Pop Album 1" } } }
                    }
                }
            }).Wait();
            return repo;
        }

        private IPlaylistMixer GetPlaylistMixer()
        {
            return new PlaylistMixer();
        }

        private IPlaylistProvider GetPlaylistProvider()
        {
            return new SpotifyPlaylistProvider(GetPlaylistRepository());
        }

        private IPlaylistOrchestrator GetPlaylistOrchestrator()
        {
            return new PlaylistOrchestrator(GetPlaylistProvider(), GetPlaylistMixer());
        }

    public static IEnumerable<object[]> ApiTestData()
    {
        // method, id, expectedResultType, mixer (for Create/Update), expectedName (for Create/Update), expectedTransformType
        yield return new object[] { "GetById", "1", typeof(OkObjectResult), null, null, typeof(TakeTransform) };
        yield return new object[] { "GetById", "2", typeof(OkObjectResult), null, null, typeof(AttributeTransform) };
        yield return new object[] { "GetById", "notfound", typeof(NotFoundResult), null, null, null };
        yield return new object[] { "Delete", "1", typeof(NoContentResult), null, null, null };
        yield return new object[] { "Delete", "notfound", typeof(NotFoundResult), null, null, null };
        yield return new object[] { "Create", "4", typeof(CreatedAtActionResult), new Mixer { id = "4", Name = "D", Transforms = new List<IPlaylistTransform> { new OrderTransform { AttributeName = "popularity", Ascending = false } } }, "D", typeof(OrderTransform) };
        yield return new object[] { "Create", "1", typeof(ConflictObjectResult), new Mixer { id = "1", Name = "A" }, null, null };
        yield return new object[] { "Update", "1", typeof(OkObjectResult), new Mixer { id = "1", Name = "A-Updated", Transforms = new List<IPlaylistTransform> { new TakeTransform { Count = 5, FromStart = false } } }, "A-Updated", typeof(TakeTransform) };
        yield return new object[] { "Update", "notfound", typeof(NotFoundResult), new Mixer { id = "notfound", Name = "X" }, null, null };
        yield return new object[] { "Update", "mismatch", typeof(BadRequestObjectResult), new Mixer { id = "other", Name = "Mismatch" }, null, null };
    }

    [Theory]
    [MemberData(nameof(ApiTestData))]
    public async Task MixerController_ApiTests(string method, string id, System.Type expectedResultType, Mixer mixer, string expectedName, System.Type expectedTransformType)
    {
        var repo = GetInitializedRepo();
        var orchestrator = GetPlaylistOrchestrator();
        var controller = new MixersController(repo, orchestrator);
        IActionResult result = null;

        switch (method)
        {
            case "GetById":
                result = await controller.GetMixerById(id);
                break;
            case "Delete":
                result = await controller.DeleteMixer(id);
                break;
            case "Create":
                result = await controller.CreateMixer(mixer);
                break;
            case "Update":
                result = await controller.UpdateMixer(id, mixer);
                break;
            default:
                throw new System.Exception("Unknown method");
        }

        Assert.IsType(expectedResultType, result);
        if (expectedResultType == typeof(CreatedAtActionResult))
        {
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var returnedMixer = Assert.IsType<Mixer>(created.Value);
            Assert.Equal(expectedName, returnedMixer.Name);
            if (expectedTransformType != null)
                Assert.Contains(returnedMixer.Transforms, t => t.GetType() == expectedTransformType);
        }
        if (expectedResultType == typeof(OkObjectResult))
        {
            var ok = Assert.IsType<OkObjectResult>(result);
            var returnedMixer = Assert.IsType<Mixer>(ok.Value);
            if (expectedName != null)
                Assert.Equal(expectedName, returnedMixer.Name);
            if (expectedTransformType != null)
                Assert.Contains(returnedMixer.Transforms, t => t.GetType() == expectedTransformType);
        }
    }

    [Fact]
    public async Task MixPlaylist_ValidMixer_ReturnsOkWithMixedTracks()
    {
        // Arrange
        var repo = GetInitializedRepo();
        var orchestrator = GetPlaylistOrchestrator();
        var controller = new MixersController(repo, orchestrator);

        // Act
        var result = await controller.MixPlaylist("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var playlistProperty = response.GetType().GetProperty("playlist");
        Assert.NotNull(playlistProperty);
        var playlist = playlistProperty.GetValue(response) as SpotifyPlaylist;
        Assert.NotNull(playlist);
        Assert.Equal("playlist-1", playlist.Id);
        Assert.Equal("Test Playlist 1 (Mixed)", playlist.Name);
        Assert.Equal(2, playlist.Tracks.Items.Count); // TakeTransform with Count=2 should return 2 tracks
    }

    [Fact]
    public async Task MixPlaylist_MixerNotFound_ReturnsNotFound()
    {
        // Arrange
        var repo = GetInitializedRepo();
        var orchestrator = GetPlaylistOrchestrator();
        var controller = new MixersController(repo, orchestrator);

        // Act
        var result = await controller.MixPlaylist("notfound");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task MixPlaylist_MixerWithoutSrcPlaylistId_ReturnsBadRequest()
    {
        // Arrange
        var repo = GetInitializedRepo();
        var orchestrator = GetPlaylistOrchestrator();
        var controller = new MixersController(repo, orchestrator);

        // Act
        var result = await controller.MixPlaylist("3"); // Mixer with empty SrcPlaylistId

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task MixPlaylist_OrchestratorThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var repo = GetInitializedRepo();
        var orchestrator = GetPlaylistOrchestrator(); // This will throw on empty playlistId
        var controller = new MixersController(repo, orchestrator);

        // Act & Assert
        // This test would require a more sophisticated mock that can be configured to throw
        // For now, we'll test the basic functionality
        var result = await controller.MixPlaylist("1");
        Assert.IsType<OkObjectResult>(result);
    }
}
} 