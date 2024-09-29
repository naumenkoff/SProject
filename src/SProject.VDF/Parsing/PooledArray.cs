using System.Buffers;
using System.Runtime.CompilerServices;

namespace SProject.VDF.Parsing;

public ref struct PooledArray<T>
{
    private readonly int _bufferSize;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PooledArray(int bufferSize)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _bufferSize = bufferSize;
        Assign(ArrayPool<T>.Shared.Rent(bufferSize));
    }

    public T[] Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private set;
    }

    public ReadOnlySpan<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private set;
    }

    public readonly ref readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Span[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Resize(int start, int length)
    {
        var array = ArrayPool<T>.Shared.Rent(length + _bufferSize);
        Span.Slice(start, length).CopyTo(array);
        Close();
        Assign(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Close()
    {
        ArrayPool<T>.Shared.Return(Array);
        Assign(System.Array.Empty<T>(), ReadOnlySpan<T>.Empty);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Assign(T[] array)
    {
        Array = array;
        Span = new ReadOnlySpan<T>(array);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Assign(T[] array, ReadOnlySpan<T> span)
    {
        Array = array;
        Span = span;
    }
}