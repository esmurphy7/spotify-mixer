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

    public static IEnumerable<object[]> ApiTestData()
    {
        // method, id, expectedResultType, mixer (for Create/Update), expectedName (for Create/Update)
        yield return new object[] { "GetById", "1", typeof(OkObjectResult), null, null };
        yield return new object[] { "GetById", "notfound", typeof(NotFoundResult), null, null };
        yield return new object[] { "Delete", "1", typeof(NoContentResult), null, null };
        yield return new object[] { "Delete", "notfound", typeof(NotFoundResult), null, null };
        yield return new object[] { "Create", "3", typeof(CreatedAtActionResult), new Mixer { id = "3", Name = "C" }, "C" };
        yield return new object[] { "Create", "1", typeof(ConflictObjectResult), new Mixer { id = "1", Name = "A" }, null };
        yield return new object[] { "Update", "1", typeof(OkObjectResult), new Mixer { id = "1", Name = "A-Updated" }, "A-Updated" };
        yield return new object[] { "Update", "notfound", typeof(NotFoundResult), new Mixer { id = "notfound", Name = "X" }, null };
        yield return new object[] { "Update", "mismatch", typeof(BadRequestObjectResult), new Mixer { id = "other", Name = "Mismatch" }, null };
    }

    [Theory]
    [MemberData(nameof(ApiTestData))]
    public async Task MixerController_ApiTests(string method, string id, System.Type expectedResultType, Mixer mixer, string expectedName)
    {
        var repo = GetInitializedRepo();
        var controller = new MixersController(repo);
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
        }
        if (expectedResultType == typeof(OkObjectResult))
        {
            var ok = Assert.IsType<OkObjectResult>(result);
            var returnedMixer = Assert.IsType<Mixer>(ok.Value);
            if (expectedName != null)
                Assert.Equal(expectedName, returnedMixer.Name);
        }
    }
} 