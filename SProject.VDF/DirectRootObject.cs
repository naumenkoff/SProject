using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class DirectRootObject : IRootObject<IRootObject, IValueObject>
{
    public required string? Key { get; init; }
    public Dictionary<string, IValueObject> ValueObjects { get; } = new Dictionary<string, IValueObject>();
    public Dictionary<string, IRootObject> RootObjects { get; } = new Dictionary<string, IRootObject>();
    IValueObject IRootObject.this[string key] => this[key];
    public IRootObject this[string key] => RootObjects[key];

    public IValueObject Get(string key)
    {
        return ValueObjects[key];
    }
}