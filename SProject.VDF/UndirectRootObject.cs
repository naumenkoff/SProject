using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class UndirectRootObject : IRootObject<IValueObject, IRootObject>
{
    public required string? Key { get; init; }
    public Dictionary<string, IValueObject> ValueObjects { get; } = new Dictionary<string, IValueObject>();
    public Dictionary<string, IRootObject> RootObjects { get; } = new Dictionary<string, IRootObject>();
    public IValueObject this[string key] => ValueObjects[key];

    public IRootObject Get(string key)
    {
        return RootObjects[key];
    }
}