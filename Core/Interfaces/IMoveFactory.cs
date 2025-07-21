using Core.Entities;

namespace Core.Interfaces;

public interface IMoveFactory
{
    Move CreateMove(int movesCount, Guid gameId, int row, int column, string player, string opponent);
}