using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class ValueObjectExtensions
{
    public static T As<T>(this IValueObject valueObject) where T : IValueObject
    {
        return (T) valueObject;
    }

    public static T GetValue<T>(this IValueObject abstraction, string key) where T : struct
    {
        switch (abstraction)
        {
            case ValueObject valueObject:
            {
                return valueObject.Key == key ? Cast<T>(valueObject.Value) : throw new KeyNotFoundException($"{key} != {valueObject.Key}");
            }
            case UndirectRootObject rootObject:
            {
                var value = rootObject[key].As<ValueObject>().Value;
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

    public static Dictionary<string, Dictionary<string, IValueObject>> BuildMap(this IRootObject rootObject)
    {
        return BuildMap(rootObject, null);
    }

    private static Dictionary<string, Dictionary<string, IValueObject>> BuildMap(IRootObject rootObject,
        Dictionary<string, Dictionary<string, IValueObject>>? dictionary)
    {
        dictionary ??= new Dictionary<string, Dictionary<string, IValueObject>>();

        if (!string.IsNullOrEmpty(rootObject.Key)) dictionary.TryAdd(rootObject.Key!, rootObject.ValueObjects);

        foreach (var valueObject in rootObject.ValueObjects)
        {
            if (valueObject.Value is IRootObject rootObj) BuildMap(rootObj, dictionary);
        }

        return dictionary;
    }
}