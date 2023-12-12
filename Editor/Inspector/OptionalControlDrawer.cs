using gomoru.su.CostumeController.Attributes;
using gomoru.su.CostumeController.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            float count = 1;
            if (property.boxedValue is not null)
            {
                count += 3;
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

            EditorGUI.PropertyField(position, property.FindPropertyRelative("Enabled"), "ON".ToGUIContent());

            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, property.FindPropertyRelative("Enabled"), "ON".ToGUIContent());

            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, property.FindPropertyRelative("Disabled"), "OFF".ToGUIContent());

        }
    }

    [CustomPropertyDrawer(typeof(Toggle<>), true)]
    internal sealed class ToggleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var r1 = position;
            var r2 = position;
            var r3 = position;
            r1.width = Mathf.Max(30f, EditorStyles.label.CalcSize(label).x);
            r2.width = EditorStyles.toggle.CalcSize(GUIUtils.GetTempContent("")).x;
            r3.width -= r2.width + 8 + r1.width + 8;
            r2.x += r1.width + 8;
            r3.x += r1.width + 8 + r2.width + 8;
            EditorGUI.LabelField(r1, label);
            var prop = property.FindPropertyRelative("Enable");
            GUIUtils.ToggleLeft(r2, prop, GUIContent.none);
            EditorGUI.BeginDisabledGroup(!prop.boolValue);

            var minMax = fieldInfo.ReflectedType.GetCustomAttributes<MinMaxAttribute>().FirstOrDefault(x => x.Target == fieldInfo.Name);
            prop = property.FindPropertyRelative("Value");
            if (minMax is not null)
            {
                var (min, max) = (minMax.Min, minMax.Max);
                var value = prop.floatValue;
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                value = EditorGUI.Slider(r3, value, min, max);
                if (EditorGUI.EndChangeCheck())
                {
                    prop.floatValue = value;
                }
            }
            else
            {
                EditorGUI.PropertyField(r3, property.FindPropertyRelative("Value"), GUIContent.none);
            }
            
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndProperty();
        }
    }
}
