namespace SProject.Vdf.Abstractions;

public interface IRootObject : IValueObject
{
    Dictionary<string, IValueObject> ValueObjects { get; }
    Dictionary<string, IRootObject> RootObjects { get; }

    new IRootObject this[string key] { get; }

    T GetRootObject<T>(string key) where T : IRootObject;
}