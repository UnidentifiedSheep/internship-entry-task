namespace Core.Exceptions.Game;

public class GameAlreadyFinishedException(Guid gameId) : BadRequestException("Игра уже окончена", new { GameId = gameId })
{
    
}