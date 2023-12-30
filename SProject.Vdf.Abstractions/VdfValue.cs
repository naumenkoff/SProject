namespace SProject.Vdf.Abstractions;

public class VdfValue : VdfObject
{
    public VdfValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Value { get; init; }

    public static implicit operator string(VdfValue vdfValue) => vdfValue.Value;
}