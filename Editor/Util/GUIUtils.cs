using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace gomoru.su.CostumeController
{
    internal static partial class GUIUtils
    {
        public static readonly GUIStyle ObjectFieldButtonStyle = typeof(EditorStyles).GetProperty("objectFieldButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetMethod.Invoke(null, null) as GUIStyle;

        private static GUIContent _tempContent;

        public static GUIContent ToGUIContent(this string text, string toolTip = null, Texture image = null)
        {
            var content = _tempContent ??= new GUIContent();
            content.text = text;
            content.tooltip = toolTip;
            content.image = image;
            return content;
        }

        public static (float Width, float Height) CalcSize(this GUIStyle style, string text)
        {
            var size = style.CalcSize(text.ToGUIContent());
            return (size.x, size.y);
        }

        public static bool ChangeCheck<T>(Func<T> func, out T value)
        {
            EditorGUI.BeginChangeCheck();
            value = func();
            return EditorGUI.EndChangeCheck();
        }
        public static void ChangeCheck<T>(SerializedProperty property, Func<T, T> func)
        {
            EditorGUI.BeginChangeCheck();
            var typed = new SerializedProperty<T>(property);
            var ret = func(typed.Value);
            if (EditorGUI.EndChangeCheck())
                typed.Value = ret;
        }

        public static bool DrawControlWithSelectionButton(Rect position, Action<Rect> drawControl) => DrawControlWithSelectionButton(position, drawControl, static (pos, action) => action(pos));

        public static bool DrawControlWithSelectionButton<TState>(Rect position, TState state, Action<Rect, TState> drawControl, bool? enabled = null)
        {
            var origPos = position;
            position.x += position.width - EditorGUIUtility.singleLineHeight;
            position.width = EditorGUIUtility.singleLineHeight;
            EditorGUIUtility.AddCursorRect(position, MouseCursor.Arrow);
            position = ObjectFieldButtonStyle.margin.Remove(position);
            bool disabled = !(enabled ?? GUI.enabled);
            EditorGUI.BeginDisabledGroup(disabled);
            bool result = GUI.Button(position, GUIContent.none, GUIStyle.none);
            EditorGUI.EndDisabledGroup();
            drawControl(origPos, state);
            
            if (Event.current.type == EventType.Repaint)
            {
                ObjectFieldButtonStyle.Draw(position, GUIContent.none, 0, !disabled, !disabled && position.Contains(Event.current.mousePosition));
            }
            return result;
        }

        public static void WithContextMenu<TState>(Rect position, TState state, Action<Rect, TState> drawControl, Action<GenericMenu> registerMenu)
        {
            if (Event.current.type == EventType.ContextClick)
            {
                if (position.Contains(Event.current.mousePosition))
                {
                    var menu = new GenericMenu();
                    registerMenu(menu);
                    menu.ShowAsContext();
                }
            }
            drawControl(position, state);
        }
    }
}
