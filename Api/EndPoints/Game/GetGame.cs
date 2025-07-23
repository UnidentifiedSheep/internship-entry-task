using Application.Mappers;
using Carter;
using Core.Dtos;
using Core.Interfaces;

namespace Api.EndPoints.Game;

public record GetGameResponse(GameDto Game);

public class GetGame : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{gameId}", async (Guid gameId, IGameService service, CancellationToken cancellationToken, bool showAsBoard = false) =>
        {
            var game = await service.GetGameAsync(gameId, cancellationToken);
            var response = new GetGameResponse(game.MapToDto(showAsBoard));
            return Results.Ok(response);
        });
    }
}