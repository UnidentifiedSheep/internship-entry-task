using Application.Game;
using Core.Dtos;

namespace Application.Mappers;

public static class GameMapper
{
    public static GameDto MapToDto(this Core.Entities.Game game, bool showAsBoard = false)
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
        if (showAsBoard)
            gameDto.Board = GameBoard.GetAsBoard(game.Moves, gameDto.FirstPlayer, gameDto.FirstPlayerSymbol, 
                gameDto.SecondPlayer, gameDto.SecondPlayerSymbol, gameDto.BoardSize);
        return gameDto;
    }

    public static MoveDto MapToDto(this Core.Entities.Move move)
    {
        return new MoveDto
        {
            Id = move.Id,
            Player = move.Player,
            X = move.X,
            Y = move.Y,
        };
    }

    public static IEnumerable<MoveDto> MapToDto(this IEnumerable<Core.Entities.Move> moves) => moves.Select(MapToDto);
    
}