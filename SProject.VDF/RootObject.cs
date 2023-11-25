using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class RootObject : IRootObject
{
    public RootObject(string key)
    {
        Key = key;
    }

    public T GetValueObject<T>(string key) where T : IValueObject
    {
        return ValueObjects[key].As<T>();
    }

    public T GetRootObject<T>(string key) where T : IRootObject
    {
        return RootObjects[key].As<T>();
    }

    public string Key { get; }

    #region Collection

    public Dictionary<string, IValueObject> ValueObjects { get; } = new Dictionary<string, IValueObject>();
    public Dictionary<string, IRootObject> RootObjects { get; } = new Dictionary<string, IRootObject>();

    #endregion

    #region Index

    public IRootObject this[string key] => RootObjects[key];
    IValueObject IValueObject.this[string key] => ValueObjects[key];

    #endregion
}