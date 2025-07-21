namespace Core.Validation;

public static class GameSettingsValidator
{
    private const int MinBoardSize = 3;
    public static void Validate(int boardSize, int winLength)
    {
        if (boardSize < MinBoardSize)
            throw new ArgumentException($"Размер поля должен быть больше {MinBoardSize}");
        if (winLength < 3 || winLength > boardSize)
            throw new ArgumentException($"Длина линии победы должна быть от 3 до {boardSize}");
    }
}