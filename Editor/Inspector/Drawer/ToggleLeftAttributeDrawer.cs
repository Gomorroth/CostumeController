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
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            using var _ = new GUIUtils.PropertyScope(position, property, ref label);

            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            GUIUtils.ChangeCheck<bool>(property, value => EditorGUI.ToggleLeft(position, label, value));
            EditorGUI.showMixedValue = false;
        }
    }
}
