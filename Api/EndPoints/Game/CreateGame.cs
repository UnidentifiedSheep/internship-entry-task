using Carter;
using Core.Configuration;
using Core.Interfaces;

namespace Api.EndPoints.Game;

public record CreateGameRequest(string FirstPlayer, string SecondPlayer, string? CurrentTurn);
public record CreateGameResponse(Guid Id);

public class CreateGame : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/games", async (IGameService gameService, CreateGameRequest request, CancellationToken cancellationToken) =>
            {
                var currentTurn = string.IsNullOrWhiteSpace(request.CurrentTurn) ? request.FirstPlayer : request.CurrentTurn!;
                var gameId = await gameService.CreateGameAsync(request.FirstPlayer, request.SecondPlayer, currentTurn, cancellationToken);
                return Results.Created($"/games/{gameId}", new CreateGameResponse(gameId));
            }).WithName("CreateGame")
            .WithTags("Games")
            .Produces<CreateGameResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Создать новую игру")
            .WithDescription("Создаёт новую игру между двумя игроками. Если не указан текущий ход — по умолчанию начинает первый игрок.");
    }
}