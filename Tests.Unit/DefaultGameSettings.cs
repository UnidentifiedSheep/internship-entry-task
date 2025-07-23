using Core.Configuration;

namespace Tests.Unit;

public static class DefaultGameSettings
{
    public static readonly GameSettings NormalSettings = new(3, 3);
    public static readonly GameSettings MediumSettings = new(5, 4);
    public static readonly GameSettings LargeSettings = new(100, 20);

    public static IEnumerable<object[]> Settings =>
        new List<object[]>
        {
            new object[] { NormalSettings },
            new object[] { MediumSettings },
            new object[] { LargeSettings }
        };
}