namespace SProject.VDF;

public static class VValueExtensions
{
    public static int? AsInt32(this VValue? vValue)
    {
        return int.TryParse(vValue?.Value, out var value) ? value : null;
    }

    public static bool AsInt32(this VValue? vValue, out int value)
    {
        return int.TryParse(vValue?.Value, out value);
    }

    public static long? AsInt64(this VValue? vValue)
    {
        return long.TryParse(vValue?.Value, out var value) ? value : null;
    }

    public static bool AsInt64(this VValue? vValue, out long value)
    {
        return long.TryParse(vValue?.Value, out value);
    }

    public static DateTimeOffset? AsDateTimeOffset(this VValue? vValue)
    {
        return vValue.AsInt64(out var seconds) ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime() : null;
    }
}