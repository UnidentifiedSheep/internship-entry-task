using Core.Models;

namespace Core.Interfaces;

public interface IGameService
{
    Task<Guid> CreateGameAsync(string firstPlayer, string secondPlayer, string currenTurn, CancellationToken cancellationToken = default);
    Task<(MoveResult? moveResult, string eTag)> MakeMoveAsync(Guid gameId, string player, int x, int y, CancellationToken cancellationToken = default);
    
    Task<Entities.Game> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default);
}