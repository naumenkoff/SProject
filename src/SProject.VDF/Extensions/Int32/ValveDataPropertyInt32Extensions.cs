// ReSharper disable once CheckNamespace

namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static int? AsInt32(this ValveDataProperty? property)
    {
        return int.TryParse(property?.Value, out var value) ? value : null;
    }

    public static int AsInt32(this ValveDataProperty? property, int fallbackValue)
    {
        return int.TryParse(property?.Value, out var value) ? value : fallbackValue;
    }

    public static bool AsInt32(this ValveDataProperty? property, out int value)
    {
        return int.TryParse(property?.Value, out value);
    }
}