using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class RootObjectExtensions
{
    public static IEnumerable<IValueObject> GetAll(this IRootObject rootObject, string key)
    {
        var valueObjects = new HashSet<IValueObject>();
        rootObject.GetAllInternal(key, valueObjects);
        return valueObjects;
    }

    private static void GetAllInternal(this IRootObject rootObject, string key, ICollection<IValueObject> valueObjects)
    {
        // in case "TARGET_KEY" { /../ }
        if (rootObject.Key == key) valueObjects.Add(rootObject);

        // in case "SOME_KEY" { /.. "TARGET_KEY" "TARGET_VALUE" ../ }
        var value = rootObject.GetValueObject<ValueObject>(key);
        if (value is not null) valueObjects.Add(value);

        foreach (var (_, dRootObject) in rootObject.RootObjects) dRootObject.GetAllInternal(key, valueObjects);
    }

    public static IEnumerable<IRootObject> GetRootObjects(this IRootObject rootObject, string key)
    {
        var rootObjects = new HashSet<IRootObject>();
        rootObject.GetRootObjectsInternal(key, rootObjects);
        return rootObjects;
    }

    private static void GetRootObjectsInternal(this IRootObject rootObject, string key, ICollection<IRootObject> rootObjects)
    {
        // in case "TARGET_KEY" { /../ }
        if (rootObject.Key == key) rootObjects.Add(rootObject);

        foreach (var (_, dRootObject) in rootObject.RootObjects) dRootObject.GetRootObjectsInternal(key, rootObjects);
    }

    public static IEnumerable<ValueObject> GetValueObjects(this IRootObject rootObject, string key)
    {
        var valueObjects = new HashSet<ValueObject>();
        rootObject.GetValueObjectsInternal(key, valueObjects);
        return valueObjects;
    }

    private static void GetValueObjectsInternal(this IRootObject rootObject, string key, ICollection<ValueObject> valueObjects)
    {
        // in case "SOME_KEY" { /.. "TARGET_KEY" "TARGET_VALUE" ../ }
        var value = rootObject.GetValueObject<ValueObject>(key);
        if (value is not null) valueObjects.Add(value);

        foreach (var (_, dRootObject) in rootObject.RootObjects) dRootObject.GetValueObjectsInternal(key, valueObjects);
    }
}