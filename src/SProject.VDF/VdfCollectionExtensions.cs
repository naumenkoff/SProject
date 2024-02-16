using System.Diagnostics.CodeAnalysis;

namespace SProject.VDF;

public static class VdfCollectionExtensions
{
    public static int? AsInt32(this VdfCollection<VdfValue>? vdfCollection, string key)
    {
        return vdfCollection?.Get(key).AsInt32();
    }

    public static bool AsInt32([NotNullWhen(true)] this VdfCollection<VdfValue>? vdfCollection, string key, out int value)
    {
        var vdfValue = vdfCollection?.Get(key);
        return vdfValue.AsInt32(out value);
    }

    public static long? AsInt64(this VdfCollection<VdfValue>? vdfCollection, string key)
    {
        return vdfCollection?.Get(key).AsInt64();
    }

    public static bool AsInt64([NotNullWhen(true)] this VdfCollection<VdfValue>? vdfCollection, string key, out long value)
    {
        var vdfValue = vdfCollection?.Get(key);
        return vdfValue.AsInt64(out value);
    }
}