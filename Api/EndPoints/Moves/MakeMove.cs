using Carter;
using Core.Interfaces;
using Core.Models;

namespace Api.EndPoints.Moves;

public record MakeMoveRequest(string Player, int Row, int Column);
public record MakeMoveResponse(MoveResult MoveResult);

public class MakeMove : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/games/{gameId}/moves", async (MakeMoveRequest request, Guid gameId, IGameService service, 
            HttpContext context, CancellationToken cancellationToken) =>
        {
            var (moveResult, eTag) = await service.MakeMoveAsync(gameId, request.Player, request.Column, request.Row, cancellationToken);
            context.Response.Headers.Append("ETag", eTag);
            if (moveResult == null)
                return Results.Ok();
            return Results.Ok(new MakeMoveResponse(moveResult.Value));
        }).WithName("MakeMove")
        .WithTags("Moves")
        .Produces<MakeMoveResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Совершить ход в игре")
        .WithDescription("""
                         Совершает ход в активной игре по координатам (Row, Column) от имени игрока.
                         Если ход уже был сделан на эту клетку, возвращается 200 OK без тела и с ETag сделанного ранее хода.
                         Если ход успешен — возвращается результат хода (победа, ничья и т.п.) и новый ETag.
                         """);
    }
}