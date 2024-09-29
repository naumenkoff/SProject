namespace SProject.VDF;

public sealed class ValveDataProperty(string key, string value) : IValveDataObject
{
    public string Value { get; init; } = value;
    public string Key { get; init; } = key;

    public static implicit operator string(ValveDataProperty property)
    {
        return property.Value;
    }
}