using SProject.VDF.Collections;

namespace SProject.VDF.Extensions;

public static partial class ValveDataEnumerableExtensions
{
    public static DateTimeOffset? AsDateTimeOffset(this ValveDataEnumerable<ValveDataProperty>? properties, string key)
    {
        return properties?.FirstOrDefault(key).AsDateTimeOffset();
    }
}