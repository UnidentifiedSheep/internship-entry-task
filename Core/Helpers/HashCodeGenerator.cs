namespace Core.Helpers;

public static class HashCodeGenerator
{
    public static string GenerateHash(params object[] args)
    {
        var hc = new HashCode();
        foreach (var arg in args) hc.Add(arg);
        return hc.ToHashCode().ToString("X8");
    }
}