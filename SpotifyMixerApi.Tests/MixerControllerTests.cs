using Xunit;
using Moq;
using SpotifyMixerApi.Controllers;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class MixerControllerTests
{
    [Fact]
    public async Task GetMixerById_ReturnsOk_WhenMixerExists()
    {
        // Arrange
        var mockRepo = new Mock<IMixerRepository>();
        var mixer = new Mixer { id = "test", Name = "Test Mixer" };
        mockRepo.Setup(r => r.GetByIdAsync("test")).ReturnsAsync(mixer);
        var controller = new MixersController(mockRepo.Object);

        // Act
        var result = await controller.GetMixerById("test");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMixer = Assert.IsType<Mixer>(okResult.Value);
        Assert.Equal("test", returnedMixer.id);
    }
} 