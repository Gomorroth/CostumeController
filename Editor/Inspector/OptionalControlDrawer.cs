using gomoru.su.CostumeController.Attributes;
using gomoru.su.CostumeController.Controls;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(OptionalControl))]
    internal class OptionalControlDrawer : PropertyDrawer
    {
        protected const float Margin = 2;

        protected virtual int LineCount { get; } = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var x = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                x += (Margin + EditorGUIUtility.singleLineHeight) * LineCount + 1;
            }

            return x;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) => OnGUI(ref position, property, label);

        public void OnGUI(ref Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            //position.y += Margin;
            var value = property.boxedValue;
            int idx = value is null ? 0 : OptionalControls.Types.IndexOf(value.GetType()) + 1;
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            
            var pos = position;
            pos.x += Margin;
            pos.width -= Margin;

            if (GUIUtils.ChangeCheck(ref idx, () => EditorGUI.Popup(pos, idx, OptionalControls.Labels)))
            {
                property.boxedValue = idx == 0 ? null : Instanciator.Create(OptionalControls.Types[idx - 1]);
                property.isExpanded = idx != 0;
            }
            EditorGUI.showMixedValue = false;

            if (idx != 0)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            }

            NewLine(ref position);
        }

        protected static void NewLine(ref Rect position)
        {
            position.y += EditorGUIUtility.singleLineHeight + Margin;
        }
    }

    [CustomPropertyDrawer(typeof(BlendshapeControl))]
    internal sealed class BlendshapeDrawer : OptionalControlDrawer
    {
        protected override int LineCount => 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(ref position, property, label);
            if (!property.isExpanded || property.managedReferenceValue is not BlendshapeControl)
                return;

            Draw(position, property);
        }

        public static void Draw(Rect position, SerializedProperty property)
        {
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Blendshape)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Enabled)));
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
