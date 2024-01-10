using gomoru.su.CostumeController.Attributes;
using gomoru.su.CostumeController.Controls;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(OptionalControl), true)]
    internal sealed class OptionalControlDrawer : PropertyDrawer
    {
        private static readonly string[] _controlLabels = new[] { "None", "Blendshape", "Material", "Color", };

        public static float GetPropertyHeight(SerializedProperty property)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (property.boxedValue is BlendshapeControl)
            {
                height += BlendshapeDrawer.GetPropertyHeight(property);
            }
            return height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetPropertyHeight(property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            var value = property.boxedValue;
            int idx = value switch
            {
                BlendshapeControl => 1,
                MaterialControl => 2,
                ColorControl => 3,
                _ => 0,
            };
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            idx = EditorGUI.Popup(position, idx, _controlLabels);
            if (EditorGUI.EndChangeCheck())
            {
                property.boxedValue = idx switch
                {
                    1 => new BlendshapeControl(),
                    2 => new MaterialControl(),
                    3 => new ColorControl(),
                    _ => null,
                };
            }
            EditorGUI.showMixedValue = false;

            if (idx == 0)
                return;

            position.y += EditorGUIUtility.singleLineHeight;

            if (fieldInfo.GetCustomAttribute<HideInInspector>() is null)
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(OptionalControl.Target)));
                position.y += EditorGUIUtility.singleLineHeight;
            }

            if (idx == 1)
                BlendshapeDrawer.OnGUI(position, property, label);
        }

        internal static class BlendshapeDrawer
        {
            public static float GetPropertyHeight(SerializedProperty property)
            {
                float count = 2;

                return EditorGUIUtility.singleLineHeight * count;
            }

            public static void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {

                EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Blendshape)));

            }
        }
    }

    [CustomPropertyDrawer(typeof(TargetObject))]
    internal sealed class TargetObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var scope = new GUIUtils.PropertyScope(position, property, ref label);

            var path = property.FindPropertyRelative(nameof(TargetObject.Path));
            var isAbsolute = property.FindPropertyRelative(nameof(TargetObject.IsAbsolute));

            if (property.serializedObject.targetObject is not Component component)
            {
                EditorGUI.PropertyField(position, path);
                return;
            }

            bool forceAbsolute = false;

            var objRect = position;
            var toggleRect = position;
            toggleRect.width = GUIUtils.CalcSize("Absolute", EditorStyles.toggle).Width;
            objRect.width -= toggleRect.width + 8;
            toggleRect.x += objRect.width + 4;

            var obj = component.gameObject;
            if (isAbsolute.boolValue)
                obj = obj.GetRootObject();

            var target = obj.Find(path.stringValue);
            if (target != null)
            {
                bool changed;
                using (new GUIUtils.ChangeCheckScope(out changed))
                    target = EditorGUI.ObjectField(objRect, target, typeof(GameObject), true) as GameObject;

                if (changed)
                {
                    var root = component.gameObject;
                    if (!root.IsChildren(target))
                    {
                        isAbsolute.boolValue = true;
                        root = root.GetRootObject();
                    }
                    if (target.GetRelativePath(root) is { } newPath)
                    {
                        path.stringValue = newPath;
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(objRect, path);
            }

            if (!forceAbsolute)

            EditorGUI.ToggleLeft(toggleRect, "Absolute", isAbsolute.boolValue);
        }
    }

}
