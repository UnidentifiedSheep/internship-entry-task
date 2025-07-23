using Core.Configuration;
using Core.Entities;
using Core.Exceptions;
using Core.Exceptions.Game;
using Core.Game;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services;

public sealed class GameService : IGameService
{
    private readonly DContext _context;
    private readonly IMoveFactory _moveFactory;
    private readonly IEtagFactory _etagFactory;
    private readonly GameSettings _gameSettings;
    
    public GameService(DContext context, IMoveFactory moveFactory, IEtagFactory etagFactory, GameSettings gameSettings)
    {
        _context = context;
        _moveFactory = moveFactory;
        _etagFactory = etagFactory;
        _gameSettings = gameSettings;
    }

    private void ValidateGameData(string firstPlayer, string secondPlayer, string currentTurn)
    {
        if (string.IsNullOrWhiteSpace(firstPlayer))
            throw new ValidationException("Имя первого игрока не должно быть пустым");
        if (string.IsNullOrWhiteSpace(secondPlayer))
            throw new ValidationException("Имя второго игрока не должно быть пустым");
        
        if (firstPlayer == secondPlayer)
            throw new ValidationException("Имя первого и второго игрока не могут быть идентичны");
        if(currentTurn != firstPlayer && currentTurn != secondPlayer)
            throw new ValidationException("Игрок который ходит первым должен быть одним из двух игроков");
    }
    
    public async Task<Guid> CreateGameAsync(string firstPlayer, string secondPlayer, string currenTurn, CancellationToken cancellationToken = default)
    {
        firstPlayer = firstPlayer.Trim();
        secondPlayer = secondPlayer.Trim();
        ValidateGameData(firstPlayer, secondPlayer, currenTurn);
        
        var gameModel = new Game
        {
            FirstPlayer = firstPlayer,
            SecondPlayer = secondPlayer,
            CurrentTurn = currenTurn,
            BoardSize = _gameSettings.BoardSize,
            WinLength = _gameSettings.WinLength,
            Etag = "no-tag"
        };
        await _context.Games.AddAsync(gameModel, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return gameModel.Id;
    }

    public async Task<(MoveResult? moveResult, string eTag)> MakeMoveAsync(Guid gameId, string player, int x, int y, CancellationToken cancellationToken = default)
    {
        player = player.Trim();
        var existingMove = await _context.Moves
            .AsNoTracking()
            .FirstOrDefaultAsync(z => z.GameId == gameId && z.X == x && z.Y == y, cancellationToken);
        if (existingMove != null)
            return (null, existingMove.Etag);
        
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var game = await _context.Games
            .Include(z => z.Moves)
            .FirstOrDefaultAsync(z => z.Id == gameId, cancellationToken);
        if (game == null) throw new GameNotFoundException(gameId);
        if (game.IsFinished)
            throw new GameAlreadyFinishedException(game.Id);
        if (player != game.CurrentTurn) throw new NotYourTurnException(game.CurrentTurn);
        
        var opponent = game.FirstPlayer == game.CurrentTurn ? game.SecondPlayer : game.FirstPlayer;
        var move = _moveFactory.CreateMove(game.Moves.Count, game.Id, y, x, player, opponent);
        var eTag = _etagFactory.GetEtag(move);
        move.Etag = eTag;
        game.Etag = eTag;
        var board = GameBoard.Create(game.Moves, _gameSettings);
        var result = board.Move(move.Y, move.X, move);
        game.IsFinished = result.IsGameEnd;
        game.WhoWon = result.IsWonMove ? move.Player : null;
        game.CurrentTurn = opponent;
        await _context.AddAsync(move, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        if (result.Value != null)
            result.Value.Id = move.Id;
        
        return (result, eTag);
    }

    public async Task<Game> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        var game = await _context.Games.AsNoTracking()
            .Include(x => x.Moves)
            .FirstOrDefaultAsync(z => z.Id == gameId, cancellationToken);
        if (game == null) throw new GameNotFoundException(gameId);
        return game;
    }
}