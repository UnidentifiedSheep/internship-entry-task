namespace Core.Dtos;

public class GameDto
{
    public Guid Id { get; set; }

    public string FirstPlayer { get; set; } = null!;
    
    public char FirstPlayerSymbol { get; set; }

    public string SecondPlayer { get; set; } = null!;
    
    public char SecondPlayerSymbol { get; set; }

    public string CurrentTurn { get; set; } = null!;

    public int BoardSize { get; set; }

    public int WinLength { get; set; }

    public string? WhoWon { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Etag { get; set; } = null!;

    public bool IsFinished { get; set; }

    public List<MoveDto> Moves { get; set; } = [];

    public char[][]? Board { get; set; }
}