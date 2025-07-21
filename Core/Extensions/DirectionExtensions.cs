using Core.Enums;

namespace Core.Extensions;

public static class DirectionExtensions
{
    public static (int dRow, int dCol) ToOffset(this Direction direction) => direction switch
    {
        Direction.Horizontal => (0, 1),
        Direction.Vertical => (1, 0),
        Direction.DiagonalRight => (1, 1),
        Direction.DiagonalLeft => (1, -1),
        _ => throw new ArgumentOutOfRangeException(nameof(direction), "Неизвестное направление")
    };
}