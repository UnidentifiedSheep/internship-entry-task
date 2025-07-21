using Application;
using Core.Configuration;
using Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

//Game config
builder.Configuration.AddEnvironmentVariables("GAME_");
builder.Services.AddOptions<GameSettings>()
    .Bind(builder.Configuration)
    .Validate(s => s.BoardSize >= 3 && s.WinLength >= 3);
builder.Services.AddSingleton<GameSettings>(sp => sp.GetRequiredService<IOptions<GameSettings>>().Value);

builder.Services.AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();


if (app.Environment.IsDevelopment())
    app.MapOpenApi();


app.UseHttpsRedirection();


app.Run();