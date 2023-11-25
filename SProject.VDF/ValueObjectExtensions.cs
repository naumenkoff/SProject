using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class ValueObjectExtensions
{
    public static T? As<T>(this IValueObject? valueObject) where T : IValueObject
    {
        return valueObject is null ? default : (T) valueObject;
    }

    public static T GetValue<T>(this IValueObject abstraction, string key)
    {
        switch (abstraction)
        {
            case ValueObject valueObject:
            {
                return valueObject.Key == key ? Cast<T>(valueObject.Value) : throw new KeyNotFoundException($"{key} != {valueObject.Key}");
            }
            case IRootObject rootObject:
            {
                var value = rootObject.GetValueObject<ValueObject>(key);
                return Cast<T>(value?.Value);
            }
            default: { throw new NotSupportedException($"{abstraction.GetType()} doesn't support getting value"); }
        }
    }

    private static T Cast<T>(string? content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        return typeof(T) switch
        {
            { } type when type == typeof(int) => (T) (object) Convert.ToInt32(content),
            { } type when type == typeof(long) => (T) (object) Convert.ToInt64(content),
            { } type when type == typeof(bool) => (T) (object) (content == "1"),
            { } type when type == typeof(string) => (T) (object) content,
            _ => throw new InvalidCastException($"Casting to {typeof(T)} isn't supported.")
        };
    }
}