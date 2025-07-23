using Core.Configuration;
using Core.Entities;
using Core.Enums;
using Core.Exceptions.Game;
using Core.Extensions;
using Core.Mappers;
using Core.Models;

namespace Core.Game;

public sealed class GameBoard
{
    private readonly Dictionary<(int Row, int Column), Move> _moves;
    private readonly GameSettings _gameSettings;

    public GameBoard(IEnumerable<Move> moves, GameSettings gameSettings)
    {
        _moves = ConvertMovesToDictionary(moves);
        _gameSettings = gameSettings;
    }

    public static char[][] GetAsBoard(IEnumerable<Move> moves, string firstPlayerName, char firstPlayerSymbol, 
        string secondPlayerName, char secondPlayerSymbol, int boardSize)
    {
        var movesList = moves.ToList();
        if (movesList.Count == 0) return [];
        if (boardSize < 3)
            throw new ArgumentOutOfRangeException(nameof(boardSize), boardSize, "Размер доски должен быть больше 3");
        char[][] result = new char[boardSize][];
        for (int i = 0; i < boardSize; i++)
            result[i] = new char[boardSize];
        
        foreach (var move in movesList)
        {
            var row = move.Y;
            var column = move.X;
            if (firstPlayerName == move.Player)
                result[row][column] = firstPlayerSymbol;
            else 
                result[row][column] = secondPlayerSymbol;
        }
        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                if (result[i][j]== '\0')
                    result[i][j] = ' ';
        return result;
    }

    public static GameBoard Create(IEnumerable<Move> moves, GameSettings settings) =>
        new GameBoard(moves, settings);

    private bool IsCellEmpty(int row, int column) => !_moves.ContainsKey((row, column));

    public MoveResult Move(int row, int column, Move move)
    {
        if (row < 0 || row >= _gameSettings.BoardSize || column < 0 || column >= _gameSettings.BoardSize)
            throw new OutOfBoardException(row, column);

        if (!IsCellEmpty(row, column))
            throw new CellIsNotEmptyException(row, column);

        _moves[(row, column)] = move;

        var (isWin, isGameEnd) = IsWinOrGameEnd(row, column, move.Player);

        return new MoveResult(move.MapToDto(), isWin, isGameEnd);
    }

    private readonly Direction[] _directions =
    [
        Direction.Horizontal,
        Direction.Vertical,
        Direction.DiagonalRight,
        Direction.DiagonalLeft
    ];
    private (bool isWin, bool isGameEnd) IsWinOrGameEnd(int row, int column, string player)
    {
        foreach (var direction in _directions)
        {
            var (dRow, dCol) = direction.ToOffset();
            int count = 1; // текущая клетка считается за 1

            count += CountDirection(row, column, dRow, dCol, player);
            count += CountDirection(row, column, -dRow, -dCol, player);

            if (count >= _gameSettings.WinLength)
                return (true, true); // победа — игра окончена
        }

        bool isBoardFull = _moves.Count >= _gameSettings.BoardSize * _gameSettings.BoardSize;
        return (false, isBoardFull); // либо продолжаем игру, либо ничья
    }

    private int CountDirection(int row, int col, int dRow, int dCol, string player)
    {
        int count = 0;

        for (int step = 1; step < _gameSettings.WinLength; step++)
        {
            int newRow = row + dRow * step;
            int newCol = col + dCol * step;

            if (newRow < 0 || newCol < 0 || newRow >= _gameSettings.BoardSize || newCol >= _gameSettings.BoardSize)
                break;

            if (_moves.TryGetValue((newRow, newCol), out var move) && move.Player == player)
                count++;
            else
                break;
        }

        return count;
    }

    private Dictionary<(int Row, int Column), Move> ConvertMovesToDictionary(IEnumerable<Move> moves)
    {
        return moves.ToDictionary(move => (move.Y, move.X));
    }
}
