namespace Core.Exceptions.Game;

public class NotYourTurnException(string currentTurn) : BadRequestException("Сейчас не ваша очередь хода.", new {CurrentTurn = currentTurn})
{
    
}