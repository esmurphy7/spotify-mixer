using SpotifyMixerApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register the repository abstraction
builder.Services.AddSingleton<IMixerRepository, InMemoryMixerRepository>();
// For production, register CosmosDbMixerRepository here instead
// builder.Services.AddSingleton<IMixerRepository>(provider => { /* Cosmos DB setup */ });

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

