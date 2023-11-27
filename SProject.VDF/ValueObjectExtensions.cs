using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class ValueObjectExtensions
{
    public static T? As<T>(this IValueObject? valueObject) where T : IValueObject
    {
        return (T?) valueObject ?? default;
    }

    public static T GetValue<T>(this IValueObject value, string key)
    {
        return value switch
        {
            ValueObject valueObject => Cast<T>(valueObject.Value),
            IRootObject rootObject => Cast<T>(rootObject.GetValueObject<ValueObject>(key)?.Value!),
            _ => throw new InvalidOperationException($"Unsupported type: {value.GetType()}")
        };
    }

    private static T Cast<T>(string value)
    {
        var type = typeof(T);
        try
        {
            if (type == typeof(int)) return (T) (object) int.Parse(value);
            if (type == typeof(uint)) return (T) (object) uint.Parse(value);
            if (type == typeof(long)) return (T) (object) long.Parse(value);
            if (type == typeof(ulong)) return (T) (object) ulong.Parse(value);
            if (type == typeof(string)) return (T) (object) value;
            throw new InvalidOperationException($"Unsupported type: {type}");
        }
        catch (Exception exception) { throw new InvalidCastException($"Unable to cast '{value}' to {type}", exception); }
    }
}