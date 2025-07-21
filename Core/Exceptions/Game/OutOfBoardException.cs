namespace Core.Exceptions.Game;

public class OutOfBoardException(int row, int column) : 
    BadRequestException("Значения X и Y выходят за границы игрового поля.", new { Row = row, Column = column })
{
    
}