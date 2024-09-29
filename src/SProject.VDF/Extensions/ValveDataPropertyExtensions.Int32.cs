using System.Diagnostics.CodeAnalysis;
using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static int? AsInt32(this ValveDataProperty? property)
    {
        return int.TryParse(property?.Value, out var value) ? value : null;
    }

    public static int? AsInt32(this ValveDataCollection<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsInt32();
    }

    public static int AsInt32(this ValveDataProperty? property, int fallbackValue)
    {
        return int.TryParse(property?.Value, out var value) ? value : fallbackValue;
    }

    public static int AsInt32(this ValveDataCollection<ValveDataProperty>? properties, string key, int fallbackValue)
    {
        var value = properties?.FirstOrDefault(key);
        return value.AsInt32(fallbackValue);
    }

    public static bool TryAsInt32([NotNullWhen(true)] this ValveDataProperty? property, out int value)
    {
        return int.TryParse(property?.Value, out value);
    }

    public static bool TryAsInt32([NotNullWhen(true)] this ValveDataCollection<ValveDataProperty>? properties,
                                  string key, out int value)
    {
        var property = properties?.FirstOrDefault(key);
        return property.TryAsInt32(out value);
    }
}