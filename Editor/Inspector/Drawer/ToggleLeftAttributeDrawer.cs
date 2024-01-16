using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(ToggleLeftAttribute))]
    internal sealed class ToggleLeftAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                return;
            }

            using var _ = new GUIUtils.PropertyScope(position, property, ref label);

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            GUIUtils.ChangeCheck<bool>(property, value => EditorGUI.ToggleLeft(position, label, value));
            EditorGUI.showMixedValue = false;
        }
    }

    [CustomPropertyDrawer(typeof(ToggleRightAttribute))]
    internal sealed class ToggleRightAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                return;
            }

            using var _ = new GUIUtils.PropertyScope(position, property, ref label);
            var margin = EditorStyles.toggle.margin.right;

            position.width = EditorStyles.toggle.CalcSize(label).x + margin * 10;

            EditorGUI.LabelField(position, label);

            var labelLength = EditorStyles.label.CalcSize(label).x;
            position.x += labelLength + margin;
            position.width -= labelLength - margin * 2;  

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            GUIUtils.ChangeCheck<bool>(property, value => EditorGUI.Toggle(position, value));
            EditorGUI.showMixedValue = false;

        }
    }
}
