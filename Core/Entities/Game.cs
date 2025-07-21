namespace Core.Entities;

public partial class Game
{
    public Guid Id { get; set; }

    public string FirstPlayer { get; set; } = null!;

    public string SecondPlayer { get; set; } = null!;

    public string CurrentTurn { get; set; } = null!;

    public int BoardSize { get; set; }

    public int WinLength { get; set; }

    public string? WhoWon { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Etag { get; set; } = null!;

    public bool IsFinished { get; set; }

    public virtual ICollection<Move> Moves { get; set; } = new List<Move>();
}
