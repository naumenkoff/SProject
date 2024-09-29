using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static DateTimeOffset? AsDateTimeOffset(this ValveDataProperty? property)
    {
        return property.TryAsInt64(out var seconds) ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime() : null;
    }

    public static DateTimeOffset? AsDateTimeOffset(this ValveDataCollection<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsDateTimeOffset();
    }

    // T? As<T>(this ValveDataProperty? property)
    // T As<T>(this ValveDataProperty? property, T fallbackValue)
    // bool As<T>([NotNullWhen(true)] this ValveDataProperty? property, out T value)
}