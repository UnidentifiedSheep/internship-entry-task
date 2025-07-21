namespace Core.Entities;

public partial class Move
{
    public Guid Id { get; set; }

    public Guid GameId { get; set; }

    public string Player { get; set; } = null!;

    public int X { get; set; }

    public int Y { get; set; }

    public DateTime MovedAt { get; set; }

    public string Etag { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;
}
