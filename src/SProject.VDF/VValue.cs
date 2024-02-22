namespace SProject.VDF;

public sealed class VValue : IVObject
{
    public VValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Value { get; init; }

    public string Key { get; init; }

    public static implicit operator string(VValue vValue)
    {
        return vValue.Value;
    }
}