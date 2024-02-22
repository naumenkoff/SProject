using System.Collections;

namespace SProject.VDF;

public sealed class VSection : IVObject, IEnumerable<VSection>
{
    public VSection(string key)
    {
        Key = key;
    }

    public VCollection<VSection> Containers { get; } = new();
    public VCollection<VValue> Objects { get; } = new();

    public VSection? this[string key] => Containers[key];

    public IEnumerator<VSection> GetEnumerator()
    {
        return Containers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string Key { get; init; }

    public VValue? GetValue(string key)
    {
        return Objects.SingleOrDefault(x => x.Key == key);
    }
}