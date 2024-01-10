#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace gomoru.su.CostumeController
{
    public ref struct ValueList<T>
    {
        private Span<T> span;
        private int count;
        private T[]? buffer;

        private ValueList(Span<T> span, int count, T[]? buffer)
        {
            this.span = span;
            this.count = count;
            this.buffer = buffer;
        }

        public static ValueList<T> Create(int minimumLength = 16)
        {
            var buffer = ArrayPool<T>.Shared.Rent(minimumLength);
            return new(buffer, 0, buffer);
        }

        public static ValueList<T> Create(Span<T> initialBuffer)
        {
            return new(initialBuffer, 0, null);
        }

        public readonly Span<T> Span => span[..count];

        public readonly int Count => count;

        public readonly int Capacity => span.Length;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            Ensure();
            //span[count++] = item;

            // skip range check
            Unsafe.Add(ref MemoryMarshal.GetReference(span), count++) = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(ReadOnlySpan<T> items)
        {
            if (items.IsEmpty)
                return;

            Ensure(items.Length);

            items.CopyTo(span[count..]);
            count += items.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Ensure(int count = 1)
        {
            int size = this.count + count;
            if (size > span.Length)
            {
                EnsureCore(size);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EnsureCore(int count)
        {
            var newBuffer = ArrayPool<T>.Shared.Rent(count);
            span.CopyTo(newBuffer);
            if (buffer != null)
                ArrayPool<T>.Shared.Return(buffer);
            span = buffer = newBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                span.Clear();
            }
            count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (buffer != null)
                ArrayPool<T>.Shared.Return(buffer);
        }
    }
}
