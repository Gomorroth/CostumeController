using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    internal class SelectionWindow : EditorWindow
    {
        public static SelectionWindow Current { get; private set; }

        private ReadOnlyMemory<string> labels; 
        private string[] buffer;

        private Action<int> callback;

        private Vector2 scrollPosition;

        public static void Show<T>(IEnumerable<T> items, Action<int> callback, string title = null, Func<T, string> factory = null)
        {
            using var list =
                typeof(T).IsValueType &&
                (Size: Unsafe.SizeOf<T>() * 32, _: 0) is { Size: < 1024 } size
                ? ValueList<T>.Create(MemoryMarshal.CreateSpan(ref Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(stackalloc byte[size.Size])), 32))
                : ValueList<T>.Create(32);

            foreach(var item in items)
            {
                list.Add(item);
            }

            Show((ReadOnlySpan<T>)list.Span, callback, title, factory);
        }

        public static void Show<T>(Span<T> items, Action<int> callback, string title = null, Func<T, string> factory = null)
            => Show((ReadOnlySpan<T>)items, callback, title, factory);

        public static void Show<T>(ReadOnlySpan<T> items, Action<int> callback, string title = null, Func<T, string> factory = null)
        {
            var window = Current;
            if (window == null)
            {
                Current = window = CreateWindow<SelectionWindow>();
            }

            window.Initialize(items, callback, factory);
            window.titleContent = new(title ?? "Select");
            window.Show();
        }

        public static void Show(ReadOnlyMemory<string> items, Action<int> callback, string title = null)
        {
            var window = Current;
            if (window == null)
            {
                Current = window = CreateWindow<SelectionWindow>();
            }

            window.labels = items;
            window.callback = callback;
            window.titleContent = new(title ?? "Select");
            window.Show();
        }

        private void Initialize<T>(ReadOnlySpan<T> items, Action<int> callback, Func<T, string> factory = null)
        {
            if (buffer != null)
                ArrayPool<string>.Shared.Return(buffer);

            buffer = ArrayPool<string>.Shared.Rent(Math.Max(32, items.Length));
            var src = items;
            var dst = buffer.AsMemory(0, items.Length).Span;
            for(int i = 0; i < dst.Length; i++)
            {
                dst[i] = factory?.Invoke(src[i]) ?? src[i].ToString();
            }
            labels = buffer.AsMemory(0, items.Length);
            this.callback = callback;
        }

        public void OnInspectorUpdate()
        {
            if (labels.IsEmpty)
                Close();
        }

        public void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            var span = labels.Span;

            var startCount = Mathf.FloorToInt(scrollPosition.y / EditorGUIUtility.singleLineHeight);
            var count = Mathf.FloorToInt(position.height / EditorGUIUtility.singleLineHeight);

            for (int i = 0; i < span.Length; i++)
            {
                var item = span[i];
                if (i < startCount || i > startCount + count)
                {
                    EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                    continue;
                }

                if (GUILayout.Button(item))
                {
                    callback?.Invoke(i);
                    Close();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        public void OnLostFocus()
        {
            Close();
        }

        public void OnDestroy()
        {
            Current = null;
            if (buffer is null)
                return;

            ArrayPool<string>.Shared.Return(buffer);
            buffer = null;
        }
    }
}
