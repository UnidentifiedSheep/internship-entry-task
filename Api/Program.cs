using Api.ExceptionHandlers;
using Application;
using Carter;
using Core.Configuration;
using Infrastructure;
using Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Game config
builder.Configuration.AddEnvironmentVariables("GAME_");
var gameSettings = new GameSettings
{
    BoardSize = builder.Configuration.GetValue<int>("BOARD_SIZE"),
    WinLength = builder.Configuration.GetValue<int>("WIN_LENGTH"),
};
builder.Services.AddSingleton(gameSettings);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddInfrastructure(builder.Configuration).AddApplication();

builder.Services.AddCarter();
var app = builder.Build();


if (app.Environment.IsDevelopment())
    app.MapOpenApi();

await EnsureDbCreatedAsync(app.Services);

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.MapCarter();
app.Run();


async Task EnsureDbCreatedAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DContext>();
    await dbContext.Database.EnsureCreatedAsync();
}