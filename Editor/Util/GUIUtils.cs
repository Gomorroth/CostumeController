using System;
using System.Buffers;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

        public static void DrawGroupSelectionField(Rect position, SerializedProperty property, GameObject gameObject)
        {
            if (DrawControlWithSelectionButton(position, property, (position, property) => EditorGUI.PropertyField(position, property)))
            {
                var items = gameObject.GetRootObject().GetComponentsInChildren<IParameterNamesProvider>(true);
                var list = ValueList<(string Group, string Name)>.Create(64);
                foreach(var item in items )
                {
                    item.GetParameterNames(ref list);
                }

                var span = list.Span;
                var buffer = ArrayPool<string>.Shared.Rent(span.Length);
                int count = 0;
                var current = property.stringValue;
                _ = buffer.Length;
                for(int i = 0; i < span.Length; i++)
                {
                    var group = span[i].Group;
                    if (string.IsNullOrEmpty(group) || group == current || buffer.AsSpan(0, count).IndexOf(group) != -1) 
                        continue;

                    buffer[count++] = group;
                }
                var labels = buffer.AsMemory(0, count);

                list.Dispose();

                if (labels.IsEmpty)
                    return;

                SelectionWindow.Show(labels, title: "Select Group", callback: i =>
                {
                    property.stringValue = labels.Span[i];
                    property.serializedObject.ApplyModifiedProperties();
                    ArrayPool<string>.Shared.Return(buffer);
                });
            }
        }

        public static void DrawTargetObject(Rect position, SerializedProperty property, GameObject container = null, Type filterType = null)
        {
            var fieldRect = position;
            var popupRect = position;
            popupRect.width = EditorStyles.popup.CalcSize("Absolute ").Width;
            fieldRect.width -= popupRect.width + 2;
            popupRect.x += fieldRect.width + 2;

            var pathProp = property.FindPropertyRelative(nameof(ObjectPath.Path));
            var target = (property.boxedValue as ObjectPath);
            var mode = target.PathMode;
            var targetObj = container == null ? null : target.GetObject(container);
            //var isAbsoluteProp = property.FindPropertyRelative(nameof(TargetObject.IsAbsolute));

            EditorGUI.BeginChangeCheck();
            var result = EditorGUI.ObjectField(fieldRect, "Target Object", targetObj, filterType ?? typeof(GameObject), true);
            if (EditorGUI.EndChangeCheck())
            {
                GameObject obj = result switch
                {
                    GameObject x => x,
                    Component x => x.gameObject,
                    _ => null,
                };

                if (obj != null)
                {
                    if (!container.IsChildren(obj))
                    {
                        mode = PathMode.Absolute;
                    }
                    pathProp.stringValue = ObjectPath.GetTargetPath(container, obj, mode);
                    //pathProp.stringValue = obj.GetRelativePath(isAbsoluteProp.boolValue ? container.GetRootObject() : container);
                }
                else
                {
                    pathProp.stringValue = null;
                }
            }

            EditorGUI.BeginChangeCheck();
            mode = (PathMode)EditorGUI.Popup(popupRect, (int)mode, new[] { "Relative", "Absolute" });
            if (EditorGUI.EndChangeCheck())
            {
                pathProp.stringValue = target.ChangePathMode(container, mode);
            }
            //GUIUtils.ChangeCheck<bool>(property.FindPropertyRelative(nameof(TargetObject.IsAbsolute)), x => EditorGUI.Popup(popupRect, x ? 1 : 0, new[] { "Relative", "Absolute" }) == 1);
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

        public static float GetHeight(SerializedProperty property, GUIContent label, bool includeChildren)
        {
            if (getHeightInternal == null)
            {
                var method = new DynamicMethod("", typeof(float), new[] { typeof(SerializedProperty), typeof(GUIContent), typeof(bool)});
                var il = method.GetILGenerator();
                var assembly = typeof(EditorGUI).Assembly;
                var types = assembly.GetTypes();
                var scriptAttributeUtilityType = types.FirstOrDefault(x => x.Name == "ScriptAttributeUtility");
                var propertyHandlerType = types.FirstOrDefault(x => x.Name == "PropertyHandler");

                // var handler = ScriptAttributeUtility.GetHandler(property);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, scriptAttributeUtilityType.GetMethod("GetHandler", BindingFlags.NonPublic | BindingFlags.Static));

                // return handler.GetHeight(property, label, includeChildren);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Callvirt, propertyHandlerType.GetMethod("GetHeight", BindingFlags.Public | BindingFlags.Instance));
                il.Emit(OpCodes.Ret);


                getHeightInternal = method.CreateDelegate(typeof(Func<SerializedProperty, GUIContent, bool, float>)) as Func<SerializedProperty, GUIContent, bool, float>;
            }

            return getHeightInternal(property, label, includeChildren);
        }

        private static Func<SerializedProperty, GUIContent, bool, float> getHeightInternal;
    }
}
