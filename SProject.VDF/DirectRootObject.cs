using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class DirectRootObject : IRootObject<IRootObject, IValueObject>
{
    public DirectRootObject(string? key)
    {
        Key = key;
    }

    public string? Key { get; }

    public IValueObject Get(string key)
    {
        return ValueObjects[key];
    }

    #region Collection

    public Dictionary<string, IValueObject> ValueObjects { get; } = new Dictionary<string, IValueObject>();
    public Dictionary<string, IRootObject> RootObjects { get; } = new Dictionary<string, IRootObject>();

    #endregion

    #region Index

    IValueObject IValueObject.this[string key] => this[key];
    public IRootObject this[string key] => RootObjects[key];

    #endregion
}