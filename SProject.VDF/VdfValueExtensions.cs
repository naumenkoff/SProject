using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class VdfValueExtensions
{
    public static int? AsInt32(this VdfValue? vdfValue)
    {
        return int.TryParse(vdfValue?.Value, out var value) ? value : null;
    }

    public static bool AsInt32(this VdfValue? vdfValue, out int value)
    {
        return int.TryParse(vdfValue?.Value, out value);
    }

    public static long? AsInt64(this VdfValue? vdfValue)
    {
        return long.TryParse(vdfValue?.Value, out var value) ? value : null;
    }

    public static bool AsInt64(this VdfValue? vdfValue, out long value)
    {
        return long.TryParse(vdfValue?.Value, out value);
    }

    public static DateTimeOffset? AsDateTimeOffset(this VdfValue? vdfValue)
    {
        return vdfValue.AsInt64(out var seconds) ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime() : null;
    }
}