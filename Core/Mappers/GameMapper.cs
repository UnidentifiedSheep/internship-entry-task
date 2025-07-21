using Core.Dtos;

namespace Core.Mappers;

public static class GameMapper
{
    public static GameDto MapToDto(this Entities.Game game, bool showAsBoard = false)
    {
        var gameDto = new GameDto
        {
            Id = game.Id,
            IsFinished = game.IsFinished,
            CreatedAt = game.CreatedAt,
            CurrentTurn = game.CurrentTurn,
            BoardSize = game.BoardSize,
            WinLength = game.WinLength,
            FirstPlayer = game.FirstPlayer,
            SecondPlayer = game.SecondPlayer,
            FirstPlayerSymbol = 'X',
            SecondPlayerSymbol = 'O',
            WhoWon = game.WhoWon,
            Etag = game.Etag,
            Moves = MapToDto(game.Moves).ToList()
        };
        return gameDto;
    }

    public static MoveDto MapToDto(this Entities.Move move)
    {
        return new MoveDto
        {
            Id = move.Id,
            Player = move.Player,
            X = move.X,
            Y = move.Y,
        };
    }

    public static IEnumerable<MoveDto> MapToDto(this IEnumerable<Entities.Move> moves) => moves.Select(MapToDto);
    
}