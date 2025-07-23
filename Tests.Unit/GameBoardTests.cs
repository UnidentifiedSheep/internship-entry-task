using Application.Game;
using Core.Configuration;
using Core.Entities;
using Core.Enums;
using Core.Exceptions.Game;

namespace Tests.Unit;

public class GameBoardTests
{
    private Move CreateMove(int x, int y, string player) =>
        new() { X = x, Y = y, Player = player };

    [Theory]
    [MemberData(nameof(DefaultGameSettings.Settings), MemberType = typeof(DefaultGameSettings))]
    public void Constructor_ShouldInitializeBoardCorrectly(GameSettings settings)
    {
        var moves = new List<Move>
        {
            CreateMove(0, 0, "Alice"),
            CreateMove(1, 1, "Bob")
        };

        var board = GameBoard.Create(moves, settings);
        var result = GameBoard.GetAsBoard(moves, "Alice", 'X', "Bob", 'O', settings.BoardSize);

        Assert.Equal('X', result[0][0]);
        Assert.Equal('O', result[1][1]);
        Assert.Equal(' ', result[settings.BoardSize - 1][settings.BoardSize - 1]);
    }

    [Theory]
    [MemberData(nameof(DefaultGameSettings.Settings), MemberType = typeof(DefaultGameSettings))]
    public void Move_ShouldPlaceSymbol_WhenCellIsEmpty(GameSettings settings)
    {
        var board = GameBoard.Create([], settings);
        var move = CreateMove(1, 1, "Alice");

        var result = board.Move(row: 1, column: 1, move);

        Assert.NotNull(result.Value);
        Assert.Equal("Alice", result.Value.Player);
        Assert.False(result.IsWonMove);
        Assert.False(result.IsGameEnd);
    }

    [Theory]
    [MemberData(nameof(DefaultGameSettings.Settings), MemberType = typeof(DefaultGameSettings))]
    public void Move_ShouldThrow_WhenCellIsOccupied(GameSettings settings)
    {
        var initial = new[] { CreateMove(1, 1, "Alice") };
        var board = GameBoard.Create(initial, settings);
        var newMove = CreateMove(1, 1, "Bob");

        Assert.Throws<CellIsNotEmptyException>(() => board.Move(1, 1, newMove));
    }

    [Theory]
    [MemberData(nameof(DefaultGameSettings.Settings), MemberType = typeof(DefaultGameSettings))]
    public void Move_ShouldThrow_WhenMoveOutOfBounds(GameSettings settings)
    {
        var board = GameBoard.Create([], settings);
        var move = CreateMove(settings.BoardSize, settings.BoardSize, "Bob");

        Assert.Throws<OutOfBoardException>(() => board.Move(settings.BoardSize, settings.BoardSize, move));
    }

    [Theory]
    [MemberData(nameof(DefaultGameSettings.Settings), MemberType = typeof(DefaultGameSettings))]
    public void Move_ShouldWin_When3InARow(GameSettings settings)
    {
        if (settings.WinLength != 3 || settings.BoardSize < 3)
            return; // Пропускаем тест, т.к. он не применим к текущим настройкам

        var moves = new[]
        {
            CreateMove(0, 0, "Alice"),
            CreateMove(1, 0, "Alice")
        };

        var board = GameBoard.Create(moves, settings);
        var winMove = CreateMove(2, 0, "Alice");

        var result = board.Move(0, 2, winMove); // row = 2, col = 0

        Assert.True(result.IsWonMove);
        Assert.True(result.IsGameEnd);
    }

    [Fact]
    public void Move_ShouldEndGame_WithDraw()
    {
        var settings = DefaultGameSettings.NormalSettings;

        var moves = new[]
        {
            CreateMove(0, 0, "Alice"),
            CreateMove(0, 1, "Bob"),
            CreateMove(0, 2, "Alice"),
            CreateMove(1, 0, "Bob"),
            CreateMove(1, 1, "Alice"),
            CreateMove(1, 2, "Bob"),
            CreateMove(2, 0, "Bob"),
            CreateMove(2, 1, "Alice"),
        };

        var board = GameBoard.Create(moves, settings);
        var lastMove = CreateMove(2, 2, "Bob");
        var result = board.Move(2, 2, lastMove);

        Assert.False(result.IsWonMove);
        Assert.True(result.IsGameEnd); // ничья
    }

    [Theory]
    [InlineData(Direction.Vertical, 0, 0, 1, 0, 2, 0)]
    [InlineData(Direction.Horizontal, 0, 0, 0, 1, 0, 2)]
    [InlineData(Direction.DiagonalRight, 0, 0, 1, 1, 2, 2)]
    [InlineData(Direction.DiagonalLeft, 0, 2, 1, 1, 2, 0)]
    public void Move_ShouldDetectWinInAllDirections(Direction _, int x1, int y1, int x2, int y2, int x3, int y3)
    {
        var settings = DefaultGameSettings.NormalSettings;

        var board = GameBoard.Create([
            CreateMove(x1, y1, "Alice"),
            CreateMove(x2, y2, "Alice")
        ], settings);

        var winMove = CreateMove(x3, y3, "Alice");
        var result = board.Move(y3, x3, winMove); // row = y, col = x

        Assert.True(result.IsWonMove);
        Assert.True(result.IsGameEnd);
    }
}