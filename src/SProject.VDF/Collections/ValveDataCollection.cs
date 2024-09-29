using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SProject.VDF.Collections;

public sealed class ValveDataCollection<T> : IEnumerable<T> where T : IValveDataObject
{
    public static readonly ValveDataCollection<T> Empty = [];
    private List<T>? _collection;

    public T? this[string key] => FirstOrDefault(key);
    public int Length => _collection?.Count ?? 0;

    [MemberNotNullWhen(false, nameof(_collection))]
    public bool IsEmpty => _collection is null || _collection.Count == 0;

    [SuppressMessage("ReSharper", "NotDisposedResourceIsReturned")]
    public IEnumerator<T> GetEnumerator()
    {
        return IsEmpty ? Enumerable.Empty<T>().GetEnumerator() : _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _collection ??= [];
        _collection.Add(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T First(string key)
    {
        return FirstOrDefault(key) ?? throw new KeyNotFoundException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? FirstOrDefault(string key)
    {
        return ProcessCollection(key, static (ref T item, T? _) => item, default, true);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Single(string key)
    {
        return SingleOrDefault(key) ?? throw new KeyNotFoundException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? SingleOrDefault(string key)
    {
        return ProcessCollection(key, static (ref T item, T? result) => result == null ? item : default, default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count(string key)
    {
        return ProcessCollection(key, static (ref T _, int result) => result + 1, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(string key)
    {
        return ProcessCollection(key, static (ref T _, bool _) => true, false, true);
    }

    private TResult ProcessCollection<TResult>(string key, ProcessDelegate<T, TResult> action, TResult initialValue,
                                               bool stopOnMatch = false)
    {
        if (IsEmpty) return initialValue;

        ref var position = ref CollectionsMarshal.AsSpan(_collection).GetPinnableReference();
        for (ref var end = ref Unsafe.Add(ref position, _collection.Count);
             Unsafe.IsAddressLessThan(ref position, ref end);
             position = ref Unsafe.Add(ref position, 1))
            if (string.Equals(position.Key, key, StringComparison.Ordinal))
            {
                initialValue = action(ref position, initialValue);
                if (stopOnMatch) break;
            }

        return initialValue;
    }

    private delegate TResult ProcessDelegate<TItem, TResult>(ref TItem item, TResult result);
}