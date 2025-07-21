using Carter;
using Core.Dtos;
using Core.Interfaces;
using Core.Mappers;

namespace Api.EndPoints.Game;

public record GetGameResponse(GameDto Game);

public class GetGame : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{gameId}", async (Guid gameId, bool showAsBoard, IGameService service, CancellationToken cancellationToken) =>
        {
            var game = await service.GetGameAsync(gameId, cancellationToken);
            var response = new GetGameResponse(game.MapToDto());
            return Results.Ok(response);
        });
    }
}