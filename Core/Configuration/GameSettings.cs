namespace Core.Configuration;

public class GameSettings
{
    public int BoardSize { get; }
    public int WinLength { get; }
    public GameSettings(int boardSize, int winLength)
    {
        if (boardSize < 3)
            throw new ArgumentOutOfRangeException(nameof(boardSize), "Board size must be at least 3");
        if (winLength < 3)
            throw new ArgumentOutOfRangeException(nameof(winLength), "WinLength must be at least 3");
        if (winLength > boardSize)
            throw new ArgumentOutOfRangeException(nameof(winLength), "WinLength must be less or equal than boardSize");
        BoardSize = boardSize;
        WinLength = winLength;
    }

}