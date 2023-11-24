namespace SProject.VDF;

public class RootObject : IValueObject
{
    public Dictionary<string, IValueObject> ValueObjects { get; set; } = new Dictionary<string, IValueObject>();
    public string? Key { get; set; }

    public IValueObject this[string key] => ValueObjects[key];

    public Dictionary<string, Dictionary<string, IValueObject>> BuildMap()
    {
        return BuildMap(this, null);
    }

    private static Dictionary<string, Dictionary<string, IValueObject>> BuildMap(RootObject rootObject,
        Dictionary<string, Dictionary<string, IValueObject>>? dictionary)
    {
        dictionary ??= new Dictionary<string, Dictionary<string, IValueObject>>();

        if (!string.IsNullOrEmpty(rootObject.Key)) dictionary.TryAdd(rootObject.Key!, rootObject.ValueObjects);

        foreach (var valueObject in rootObject.ValueObjects)
        {
            if (valueObject.Value is RootObject rootObj) BuildMap(rootObj, dictionary);
        }

        return dictionary;
    }
}