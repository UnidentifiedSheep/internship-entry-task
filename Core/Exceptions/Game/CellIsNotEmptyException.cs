namespace Core.Exceptions.Game;

public class CellIsNotEmptyException(int row, int column) : BadRequestException("Клетка не пуста.", new { Row = row, Column = column })
{
    
}