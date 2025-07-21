using Core.Abstract;
using Core.Entities;
using Core.Interfaces;

namespace Application.Factory;

public class MoveFactory : IMoveFactory
{
    private readonly RandomizerBase _randomizer;

    public MoveFactory(RandomizerBase randomizer)
    {
        _randomizer = randomizer;
    }

    public Move CreateMove(int movesCount, Guid gameId, int row, int column, string player, string opponent)
    {
        var move = new Move
        {
            Y = row,
            X = column,
            GameId = gameId,
            Player = player,
        };
        if ((movesCount + 1) % 3 == 0 && _randomizer.TryChance(0.1))
            move.Player = opponent;
        return move;
    }
}
