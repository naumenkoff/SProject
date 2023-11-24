namespace SProject.VDF;

public static class ValueObjectExtensions
{
    public static RootObject? AsRootObject(this IValueObject valueObject)
    {
        return valueObject as RootObject;
    }

    public static ValueObject? AsValueObject(this IValueObject valueObject)
    {
        return valueObject as ValueObject;
    }

    public static T GetValue<T>(this IValueObject abstraction, string key) where T : struct
    {
        switch (abstraction)
        {
            case ValueObject valueObject:
            {
                return valueObject.Key == key ? Cast<T>(valueObject.Value) : throw new KeyNotFoundException($"{key} != {valueObject.Key}");
            }
            case RootObject rootObject:
            {
                var value = rootObject[key].AsValueObject()?.Value;
                return Cast<T>(value);
            }
            default: { throw new NotSupportedException($"{abstraction.GetType()} doesn't support getting value"); }
        }
    }

    private static T Cast<T>(string? content) where T : struct
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        return typeof(T) switch
        {
            { } type when type == typeof(int) => (T) (object) Convert.ToInt32(content),
            { } type when type == typeof(long) => (T) (object) Convert.ToInt64(content),
            { } type when type == typeof(bool) => (T) (object) (content == "1"),
            _ => throw new InvalidCastException($"Casting to {typeof(T)} isn't supported.")
        };
    }
}