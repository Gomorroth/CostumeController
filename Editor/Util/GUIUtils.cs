using System;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    internal static partial class GUIUtils
    {
        private static GUIContent _tempContent;

        public static GUIContent ToGUIContent(this string text, string toolTip = null, Texture image = null)
        {
            var content = _tempContent ??= new GUIContent();
            content.text = text;
            content.tooltip = toolTip;
            content.image = image;
            return content;
        }

        public static (float Width, float Height) CalcSize(string text, GUIStyle style) => style.CalcSize(text.ToGUIContent()).AsTuple();

        public static bool ChangeCheck<T>(ref T value, Func<T> func)
        {
            EditorGUI.BeginChangeCheck();
            var ret = func();
            if (!EditorGUI.EndChangeCheck())
                return false;
            value = ret;
            return true;
        }
        public static void ChangeCheck<T>(SerializedProperty property, Func<T, T> func)
        {
            EditorGUI.BeginChangeCheck();
            var typed = new SerializedProperty<T>(property);
            var ret = func(typed.Value);
            if (EditorGUI.EndChangeCheck())
                typed.Value = ret;
        }
    }
}
