using System.Collections;

namespace SProject.Vdf.Abstractions;

public class VdfContainer : VdfObject, IEnumerable<VdfContainer>
{
    public VdfContainer(string key)
    {
        Key = key;
    }

    public VdfCollection<VdfContainer> Containers { get; } = new VdfCollection<VdfContainer>();
    public VdfCollection<VdfValue> Objects { get; } = new VdfCollection<VdfValue>();

    public VdfContainer? this[string key] => Containers[key];

    public IEnumerator<VdfContainer> GetEnumerator()
    {
        return Containers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}