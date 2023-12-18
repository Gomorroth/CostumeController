using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace gomoru.su.CostumeController
{
    internal static class RuntimeUtils
    {
        public static void MarkDirty(this Object obj)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
#endif
        }

        public static void RecordObject(Object obj, string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCompleteObjectUndo(obj, name);
#endif
        }

        [ThreadStatic]
        private static string[] _relativePathBuffer;

        public static string GetRelativePath(this GameObject gameObject, GameObject relativeTo)
        {
            if (relativeTo == gameObject) return "";

            var buffer = _relativePathBuffer ??= new string[256];
            int i = buffer.Length - 1;

            while (gameObject != relativeTo && gameObject != null)
            {
                if (i < 0)
                {
                    var newBuffer = new string[buffer.Length * 2];
                    buffer.CopyTo(newBuffer.AsSpan(buffer.Length));
                    i += buffer.Length;
                    _relativePathBuffer = buffer = newBuffer;
                }
                buffer[i--] = gameObject.name;
                gameObject = gameObject.transform.parent?.gameObject;
            }

            if (gameObject == null && relativeTo != null) return null;

            i++;

            return string.Join("/", buffer, i, buffer.Length - i);
        }

        public static GameObject GetRootObject(this GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            var tr = gameObject.transform;
            while (tr.parent != null)
            {
                tr = tr.parent;
            }
            return tr.gameObject;
        }

        public static GameObject Find(this GameObject obj, string path)
        {
            return obj?.transform.Find(path)?.gameObject;
        }

        public static bool Contains<T>(this List<T> list, Predicate<T> condition)
        {
            foreach (var item in list.AsSpan())
            {
                if (condition(item))
                    return true;
            }
            return false;
        }

        public static int IndexOf<T>(this Span<T> span, Predicate<T> condition) => ((ReadOnlySpan<T>)span).IndexOf(condition);

        public static int IndexOf<T>(this ReadOnlySpan<T> span, Predicate<T> condition)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (condition(span[i]))
                    return i;
            }
            return -1;
        }

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
    }
}
