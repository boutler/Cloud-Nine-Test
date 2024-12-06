using Microsoft.EntityFrameworkCore;
using TechTestBackend.DataBaseContext;
using TechTestBackend.Repositories;
using TechTestBackend.Services.Spotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<SongstorageContext>(options => options.UseInMemoryDatabase("Songstorage"));

// Scoped services
builder.Services.AddScoped<SpotifyService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<SongstorageContext>();
builder.Services.AddScoped<SongStorageRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();