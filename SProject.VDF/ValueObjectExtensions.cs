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
            case UndirectRootObject undirectRootObject:
            {
                var value = undirectRootObject[key].As<ValueObject>().Value;
                return Cast<T>(value);
            }
            case DirectRootObject directRootObject:
            {
                var value = directRootObject.Get(key).As<ValueObject>().Value;
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

    public static IEnumerable<IRootObject> GetSection(this IRootObject rootObject, string key)
    {
        var list = new List<IRootObject>();

        foreach (var val in rootObject.RootObjects)
        {
            if (val.Key == key) list.Add(val.Value);

            var searchNext = val.Value.GetSection(key);
            list.AddRange(searchNext);
        }

        if (rootObject.ValueObjects.Any(val => val.Key == key)) list.Add(rootObject);

        return list;
    }
}