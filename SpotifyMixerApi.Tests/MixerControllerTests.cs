using Xunit;
using Moq;
using SpotifyMixerApi.Controllers;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MixerControllerTests
{
    [Fact]
    public async Task GetMixerById_ReturnsOk_WhenMixerExists()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "test", Name = "Test Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("test")).ReturnsAsync(mixer);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.GetMixerById("test");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMixer = Assert.IsType<Mixer>(okResult.Value);
        Assert.Equal("test", returnedMixer.id);
    }

    [Fact]
    public async Task GetMixerById_ReturnsNotFound_WhenMixerDoesNotExist()
    {
        var mockRepo = new Mock<IMixerRepository>();
        mockRepo.Setup(r => r.GetByIdAsync("notfound")).ReturnsAsync((Mixer?)null);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.GetMixerById("notfound");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAllMixers_ReturnsAllMixers()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixers = new List<Mixer> { new Mixer { id = "1", Name = "A" }, new Mixer { id = "2", Name = "B" } };
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mixers);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.GetAllMixers();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMixers = Assert.IsType<List<Mixer>>(okResult.Value);
        Assert.Equal(2, returnedMixers.Count);
    }

    [Fact]
    public async Task CreateMixer_ReturnsConflict_WhenMixerExists()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "exists", Name = "Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("exists")).ReturnsAsync(mixer);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.CreateMixer(mixer);

        var conflict = Assert.IsType<ConflictObjectResult>(result);
        Assert.Contains("already exists", conflict.Value.ToString());
    }

    [Fact]
    public async Task CreateMixer_ReturnsCreated_WhenMixerDoesNotExist()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "new", Name = "New Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("new")).ReturnsAsync((Mixer?)null);
        mockRepo.Setup(r => r.AddAsync(mixer)).Returns(Task.CompletedTask);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.CreateMixer(mixer);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returnedMixer = Assert.IsType<Mixer>(created.Value);
        Assert.Equal("new", returnedMixer.id);
    }

    [Fact]
    public async Task UpdateMixer_ReturnsBadRequest_WhenIdMismatch()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "id1", Name = "Mixer" };
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.UpdateMixer("id2", mixer);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("do not match", badRequest.Value.ToString());
    }

    [Fact]
    public async Task UpdateMixer_ReturnsNotFound_WhenMixerDoesNotExist()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "id1", Name = "Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync((Mixer?)null);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.UpdateMixer("id1", mixer);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateMixer_ReturnsOk_WhenMixerExists()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "id1", Name = "Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(mixer);
        mockRepo.Setup(r => r.UpdateAsync(mixer)).Returns(Task.CompletedTask);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.UpdateMixer("id1", mixer);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMixer = Assert.IsType<Mixer>(okResult.Value);
        Assert.Equal("id1", returnedMixer.id);
    }

    [Fact]
    public async Task DeleteMixer_ReturnsNotFound_WhenMixerDoesNotExist()
    {
        var mockRepo = new Mock<IMixerRepository>();
        mockRepo.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync((Mixer?)null);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.DeleteMixer("id1");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteMixer_ReturnsNoContent_WhenMixerExists()
    {
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "id1", Name = "Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("id1")).ReturnsAsync(mixer);
        mockRepo.Setup(r => r.DeleteAsync("id1")).Returns(Task.CompletedTask);
        var controller = new MixersController(mockRepo.Object);

        var result = await controller.DeleteMixer("id1");

        Assert.IsType<NoContentResult>(result);
    }
} 