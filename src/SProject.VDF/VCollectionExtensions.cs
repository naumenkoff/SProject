using System.Diagnostics.CodeAnalysis;

namespace SProject.VDF;

public static class VCollectionExtensions
{
    public static int? AsInt32(this VCollection<VValue>? vCollection, string key)
    {
        return vCollection?.FirstOrDefault(key).AsInt32();
    }

    public static bool AsInt32([NotNullWhen(true)] this VCollection<VValue>? vCollection, string key,
        out int value)
    {
        var vValue = vCollection?.FirstOrDefault(key);
        return vValue.AsInt32(out value);
    }

    public static long? AsInt64(this VCollection<VValue>? vCollection, string key)
    {
        return vCollection?.FirstOrDefault(key).AsInt64();
    }

    public static bool AsInt64([NotNullWhen(true)] this VCollection<VValue>? vCollection, string key,
        out long value)
    {
        var vValue = vCollection?.FirstOrDefault(key);
        return vValue.AsInt64(out value);
    }
}