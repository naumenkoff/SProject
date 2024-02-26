namespace SProject.VDF.Extensions;

public static partial class ValveDataPropertyExtensions
{
    public static DateTimeOffset? AsDateTimeOffset(this ValveDataProperty? property)
    {
        return property.AsInt64(out var seconds) ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime() : null;
    }
}