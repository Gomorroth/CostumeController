using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace gomoru.su.CostumeController
{
    partial class RuntimeUtils
    {
        public static Span<T> AsSpan<T>(this List<T> list)
        {
            var dummy = Unsafe.As<DummyList<T>>(list);
            return dummy.Array.AsSpan(0, list.Count);
        }

        private sealed class DummyList<T>
        {
            public T[] Array;
            public int Count;
        }

        public static class SharedList<T>
        {
            private static readonly List<T> instance = new(128);
            public static List<T> Instance
            {
                get
                {
                    if (instance.Count > instance.Capacity * 0.7f)
                        instance.Clear();
                    return instance;
                }
            }


        }
    }
}
