using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public class ValueObject : IValueObject
{
    public ValueObject(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Value { get; }
    public string Key { get; }
    public IValueObject this[string key] => this;
}