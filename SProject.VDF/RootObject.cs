namespace SProject.VDF;

public class RootObject : IValueObject
{
    public List<IValueObject> ValueObjects { get; set; } = new List<IValueObject>();
    public string? Key { get; set; }

    public Dictionary<string, List<IValueObject>> BuildMap()
    {
        return BuildMap(this, null);
    }

    private static Dictionary<string, List<IValueObject>> BuildMap(RootObject rootObject, Dictionary<string, List<IValueObject>>? dictionary)
    {
        dictionary ??= new Dictionary<string, List<IValueObject>>();

        if (string.IsNullOrEmpty(rootObject.Key)) throw new NullReferenceException(nameof(rootObject.Key));
        dictionary.TryAdd(rootObject.Key!, rootObject.ValueObjects);
    
        foreach (var valueObject in rootObject.ValueObjects)
        {
            if (valueObject is RootObject rootObj)
            {
                BuildMap(rootObj, dictionary);
            }
        }

        return dictionary;
    }
}