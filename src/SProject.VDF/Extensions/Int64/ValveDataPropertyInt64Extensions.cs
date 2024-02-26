// ReSharper disable once CheckNamespace

namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static long? AsInt64(this ValveDataProperty? property)
    {
        return long.TryParse(property?.Value, out var value) ? value : null;
    }

    public static long AsInt64(this ValveDataProperty? property, long fallbackValue)
    {
        return long.TryParse(property?.Value, out var value) ? value : fallbackValue;
    }

    public static bool AsInt64(this ValveDataProperty? property, out long value)
    {
        return long.TryParse(property?.Value, out value);
    }
}