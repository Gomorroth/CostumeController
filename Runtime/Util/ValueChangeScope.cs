using System;
using System.Runtime.InteropServices;

namespace gomoru.su.CostumeController
{
    internal readonly ref struct ValueChangeScope<T>
    {
        private readonly Span<T> span;
        private readonly T value;

        public ValueChangeScope(ref T field, T newValue)
        {
            this.value = field;
            field = newValue;

            span = MemoryMarshal.CreateSpan(ref field, 1);
        }

        public void Dispose() => MemoryMarshal.GetReference(span) = value;
    }
}
