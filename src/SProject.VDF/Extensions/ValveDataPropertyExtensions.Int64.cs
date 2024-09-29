using System.Diagnostics.CodeAnalysis;
using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static long? AsInt64(this ValveDataProperty? property)
    {
        return long.TryParse(property?.Value, out var value) ? value : null;
    }

    public static long? AsInt64(this ValveDataCollection<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsInt64();
    }

    public static long AsInt64(this ValveDataProperty? property, long fallbackValue)
    {
        return long.TryParse(property?.Value, out var value) ? value : fallbackValue;
    }

    public static long AsInt64(this ValveDataCollection<ValveDataProperty>? properties, string key, long fallbackValue)
    {
        var property = properties?.FirstOrDefault(key);
        return property.AsInt64(fallbackValue);
    }

    public static bool TryAsInt64([NotNullWhen(true)] this ValveDataProperty? property, out long value)
    {
        return long.TryParse(property?.Value, out value);
    }

    public static bool TryAsInt64([NotNullWhen(true)] this ValveDataCollection<ValveDataProperty>? properties,
                                  string key, out long value)
    {
        var property = properties?.FirstOrDefault(key);
        return property.TryAsInt64(out value);
    }
}