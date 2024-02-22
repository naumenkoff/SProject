using System.Collections;
using System.Runtime.InteropServices;

namespace SProject.VDF;

public sealed class VCollection<T> : IEnumerable<T> where T : IVObject
{
    private readonly List<T> _collection = new();

    public T? this[string key] => FirstOrDefault(key);

    public int Count => _collection.Count;
    public bool IsEmpty => _collection.Count == 0;
    public bool IsNotEmpty => _collection.Count != 0;

    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public T? FirstOrDefault(string key)
    {
        foreach (var value in CollectionsMarshal.AsSpan(_collection))
            if (value.Key == key)
                return value;

        return default;
    }

    public IEnumerable<T> Enumerate(string key)
    {
        return _collection.Where(x => x.Key == key);
    }

    public void Add(T item)
    {
        _collection.Add(item);
    }
}