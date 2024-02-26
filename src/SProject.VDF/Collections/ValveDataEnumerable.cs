using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SProject.VDF.Collections;

public class ValveDataEnumerable<T> : IEnumerable<T> where T : IValveDataObject
{
    public static readonly ValveDataEnumerable<T> Empty = new();

    protected List<T>? Collection;

    protected ValveDataEnumerable()
    {
    }

    public T? this[string key] => FirstOrDefault(key);

    public int Count => Collection?.Count ?? 0;

    [MemberNotNullWhen(false, nameof(Collection))]
    public bool IsEmpty => Collection is null || Collection.Count == 0;

    public IEnumerator<T> GetEnumerator()
    {
        return Collection?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<T> Enumerate(string key)
    {
        return IsEmpty ? Enumerable.Empty<T>() : Collection.Where(x => x.Key == key);
    }

    public T? FirstOrDefault(string key)
    {
        if (IsEmpty) return default;

        foreach (var value in CollectionsMarshal.AsSpan(Collection))
            if (string.Equals(value.Key, key, StringComparison.Ordinal))
                return value;

        return default;
    }
}