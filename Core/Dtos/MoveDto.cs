namespace Core.Dtos;

public class MoveDto
{
    public Guid Id { get; set; }
    
    public string Player { get; set; } = null!;
    
    public int X { get; set; }

    public int Y { get; set; }
}