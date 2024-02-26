namespace SProject.VDF;

public sealed class ValveDataProperty : IValveDataObject
{
    public ValveDataProperty(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Value { get; init; }

    public string Key { get; init; }

    public static implicit operator string(ValveDataProperty property)
    {
        return property.Value;
    }
}