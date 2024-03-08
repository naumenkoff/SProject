using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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

    public int Length => Collection?.Count ?? 0;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T First(string key)
    {
        return FirstOrDefault(key) ?? throw new KeyNotFoundException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? FirstOrDefault(string key)
    {
        if (IsEmpty) return default;

        ref var start = ref CollectionsMarshal.AsSpan(Collection).GetPinnableReference();
        ref var end = ref Unsafe.Add(ref start, Collection.Count);

        for (; Unsafe.IsAddressLessThan(ref start, ref end); start = ref Unsafe.Add(ref start, 1))
            if (string.Equals(start.Key, key, StringComparison.Ordinal))
                return start;

        return default;
    }

    public T Single(string key)
    {
        return SingleOrDefault(key) ?? throw new KeyNotFoundException();
    }

    public T? SingleOrDefault(string key)
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count(string key)
    {
        if (IsEmpty) return 0;

        ref var start = ref CollectionsMarshal.AsSpan(Collection).GetPinnableReference();
        ref var end = ref Unsafe.Add(ref start, Collection.Count);

        var count = 0;

        for (; Unsafe.IsAddressLessThan(ref start, ref end); start = ref Unsafe.Add(ref start, 1))
            if (string.Equals(start.Key, key, StringComparison.Ordinal))
                count++;

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(string key)
    {
        if (IsEmpty) return false;

        ref var start = ref CollectionsMarshal.AsSpan(Collection).GetPinnableReference();
        ref var end = ref Unsafe.Add(ref start, Collection.Count);

        for (; Unsafe.IsAddressLessThan(ref start, ref end); start = ref Unsafe.Add(ref start, 1))
            if (string.Equals(start.Key, key, StringComparison.Ordinal))
                return true;

        return false;
    }
}