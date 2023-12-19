using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    static partial class GUIUtils
    {
        public ref struct PropertyScope
        {
            public PropertyScope(Rect position, SerializedProperty property, GUIContent content) : this(position, property, ref content) { }
            public PropertyScope(Rect position, SerializedProperty property, ref GUIContent label)
            {
                label = EditorGUI.BeginProperty(position, label, property);
            }

            public void Dispose() => EditorGUI.EndProperty();
        }

        public readonly ref struct ChangeCheckScope
        {
            // private ref bool _isChanged;
            private readonly Span<bool> _isChanged;

            public ChangeCheckScope(out bool isChanged)
            {
                isChanged = false;
                _isChanged = MemoryMarshal.CreateSpan(ref isChanged, 1);
                EditorGUI.BeginChangeCheck();
            }

            public void Dispose() => MemoryMarshal.GetReference(_isChanged) = EditorGUI.EndChangeCheck();
        }
    }
}
