using System.Diagnostics.CodeAnalysis;
using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataEnumerableExtensions
{
    public static int? AsInt32(this ValveDataEnumerable<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsInt32();
    }

    public static int AsInt32(this ValveDataEnumerable<ValveDataProperty>? properties, string key, int fallbackValue)
    {
        var value = properties?.FirstOrDefault(key);
        return value.AsInt32(fallbackValue);
    }

    public static bool AsInt32([NotNullWhen(true)] this ValveDataEnumerable<ValveDataProperty>? properties, string key,
        out int value)
    {
        var property = properties?.FirstOrDefault(key);
        return property.AsInt32(out value);
    }
}