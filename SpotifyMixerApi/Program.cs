using SpotifyMixerApi.Repositories;
using SpotifyMixerApi.Services;
using Microsoft.Azure.Cosmos;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Models.Spotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
});

// Register the repository abstraction
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IRepository<string, Mixer>>(provider =>
    {
        var config = provider.GetRequiredService<IConfiguration>();
        var connectionString = config["CosmosDb:ConnectionString"];
        var databaseId = config["CosmosDb:DatabaseId"];
        var containerId = config["CosmosDb:ContainerId"];
        var cosmosClient = new CosmosClient(connectionString);
        var db = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).GetAwaiter().GetResult();
        db.Database.CreateContainerIfNotExistsAsync(containerId, "/id").GetAwaiter().GetResult();
        var container = cosmosClient.GetContainer(databaseId, containerId);
        return new CosmosDbRepository<string, Mixer>(container);
    });
    builder.Services.AddScoped<IRepository<string, SpotifyPlaylist>, InMemoryRepository<string, SpotifyPlaylist>>();
}
else
{
    builder.Services.AddSingleton<IRepository<string, Mixer>, InMemoryRepository<string, Mixer>>();
    builder.Services.AddScoped<IRepository<string, SpotifyPlaylist>, InMemoryRepository<string, SpotifyPlaylist>>();
}

// Register playlist services
builder.Services.AddScoped<IPlaylistMixer, PlaylistMixer>();
builder.Services.AddScoped<IPlaylistProvider, SpotifyPlaylistProvider>();
builder.Services.AddScoped<IPlaylistOrchestrator, PlaylistOrchestrator>();

// Register repositories


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

