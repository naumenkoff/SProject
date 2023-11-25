namespace SProject.Vdf.Abstractions;

public interface IRootObject<out TDirect, out TIndirect> : IRootObject where TDirect : IValueObject where TIndirect : IValueObject
{
    TDirect this[string key] { get; }
    TIndirect Get(string key);
}

public interface IRootObject : IValueObject {
    Dictionary<string, IValueObject> ValueObjects { get; }
    Dictionary<string, IRootObject> RootObjects { get; }
}