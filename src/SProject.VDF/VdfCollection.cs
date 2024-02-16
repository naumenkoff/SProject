using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SProject.VDF;

public class VdfCollection<T> : IEnumerable<T> where T : VdfObject
{
    private readonly HashSet<T> _hashSet = new HashSet<T>();

    public T? this[string key] => _hashSet.FirstOrDefault(x => x.Key == key);

    public int Count => _hashSet.Count;
    public bool IsEmpty => _hashSet.Count == 0;
    public bool IsNotEmpty => _hashSet.Count != 0;

    public IEnumerator<T> GetEnumerator()
    {
        return _hashSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public T? Get(string key)
    {
        return _hashSet.SingleOrDefault(x => x.Key == key);
    }

    public bool TryGet(string key, [MaybeNullWhen(false)] out T item)
    {
        item = _hashSet.SingleOrDefault(x => x.Key == key);
        return item is not null;
    }

    public IEnumerable<T> Enumerate(string key)
    {
        return _hashSet.Where(x => x.Key == key);
    }

    public void Add(T item)
    {
        _hashSet.Add(item);
    }
}