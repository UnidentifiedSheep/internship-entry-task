namespace Core.Exceptions.Game;

public class GameNotFoundException(Guid id) : NotFoundException("Не удалось найти игру.", new { Id = id })
{
    
}