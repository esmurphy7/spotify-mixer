using Xunit;
using SpotifyMixerApi.Controllers;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MixerControllerTheoryTests
{
    private InMemoryMixerRepository GetInitializedRepo()
    {
        var repo = new InMemoryMixerRepository();
        repo.AddAsync(new Mixer { id = "1", Name = "A" }).Wait();
        repo.AddAsync(new Mixer { id = "2", Name = "B" }).Wait();
        return repo;
    }

    [Theory]
    [InlineData("GetById", "1", typeof(OkObjectResult))]
    [InlineData("GetById", "notfound", typeof(NotFoundResult))]
    [InlineData("Delete", "1", typeof(NoContentResult))]
    [InlineData("Delete", "notfound", typeof(NotFoundResult))]
    public async Task MixerController_ApiTests(string method, string id, System.Type expectedResultType)
    {
        var repo = GetInitializedRepo();
        var controller = new MixersController(repo);

        IActionResult result = method switch
        {
            "GetById" => await controller.GetMixerById(id),
            "Delete" => await controller.DeleteMixer(id),
            _ => throw new System.Exception("Unknown method")
        };

        Assert.IsType(expectedResultType, result);
    }
} 