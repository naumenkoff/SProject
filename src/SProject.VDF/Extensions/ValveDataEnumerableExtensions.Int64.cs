using System.Diagnostics.CodeAnalysis;
using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataEnumerableExtensions
{
    public static long? AsInt64(this ValveDataEnumerable<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsInt64();
    }

    public static long AsInt64(this ValveDataEnumerable<ValveDataProperty>? properties, string key, long fallbackValue)
    {
        var property = properties?.FirstOrDefault(key);
        return property.AsInt64(fallbackValue);
    }

    public static bool AsInt64([NotNullWhen(true)] this ValveDataEnumerable<ValveDataProperty>? properties,
        string key, out long value)
    {
        var property = properties?.FirstOrDefault(key);
        return property.AsInt64(out value);
    }
}