using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class UndirectRootObject : IRootObject<IValueObject, IRootObject>
{
    public UndirectRootObject(string? key)
    {
        Key = key;
    }

    #region Index

    public IValueObject this[string key] => ValueObjects[key];

    #endregion

    public string? Key { get; }

    public IRootObject Get(string key)
    {
        return RootObjects[key];
    }

    #region Collection

    public Dictionary<string, IValueObject> ValueObjects { get; } = new Dictionary<string, IValueObject>();
    public Dictionary<string, IRootObject> RootObjects { get; } = new Dictionary<string, IRootObject>();

    #endregion
}