using gomoru.su.CostumeController.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(IOptionalControl), true)]
    internal sealed class OptionalControlDrawer : PropertyDrawer
    {
        private static readonly string[] _controlLabels = new[] { "None", "Blendshape", "Material", "Color", };

        public static float GetPropertyHeight(SerializedProperty property)
        {
            float count = 1;
            if (property.boxedValue is not null)
            {
                count++;
            }
            return EditorGUIUtility.singleLineHeight * count;
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

            EditorGUI.PropertyField(position, property.FindPropertyRelative("Enabled"));

        }
    }
}
