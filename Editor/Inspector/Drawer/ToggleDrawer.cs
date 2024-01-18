using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(Toggle<>))]
    internal sealed class ToggleDrawer : PropertyDrawer
    {
        private static readonly GUIContent[] onoffLabels = new GUIContent[] { new("OFF"), new("ON") };
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var _ = new GUIUtils.PropertyScope(position, property, ref label);

            var labelRect = position;
            var toggleRect = position;
            var fieldRect = position;

            labelRect.width = EditorGUIUtility.labelWidth;
            toggleRect.width = EditorStyles.toggle.CalcSize(string.Empty).Width;
            fieldRect.width -= labelRect.width + toggleRect.width + 4;

            toggleRect.x += labelRect.width + 2;
            fieldRect.x += labelRect.width + 2 + toggleRect.width + 2;

            EditorGUI.LabelField(labelRect, label);
            var enableProperty = property.FindPropertyRelative(nameof(Toggle<object>.Enable));
            
            EditorGUI.showMixedValue = enableProperty.hasMultipleDifferentValues;
            GUIUtils.ChangeCheck<bool>(enableProperty, value => EditorGUI.ToggleLeft(toggleRect, GUIContent.none, value));
            EditorGUI.showMixedValue = false;

            using (new GUIUtils.DisableScope(!enableProperty.boolValue))
            {
                if (!enableProperty.boolValue)
                {
                    EditorGUI.LabelField(fieldRect, "Use Current Value", EditorStyles.textArea);
                    return;
                }
                var valueProperty = property.FindPropertyRelative(nameof(Toggle<object>.Value));
                EditorGUI.showMixedValue = enableProperty.hasMultipleDifferentValues;

                if (valueProperty.propertyType == SerializedPropertyType.Float)
                    GUIUtils.ChangeCheck<float>(valueProperty, value => EditorGUI.Slider(fieldRect, value, 0, 100));
                else if (valueProperty.propertyType == SerializedPropertyType.Boolean)
                    GUIUtils.ChangeCheck<bool>(valueProperty, value => EditorGUI.Popup(fieldRect, value ? 1 : 0, onoffLabels) != 0);
                else 
                    EditorGUI.PropertyField(fieldRect, valueProperty, GUIContent.none);

                EditorGUI.showMixedValue = false;
            }
        }
    }

    [CustomPropertyDrawer(typeof(GroupSelectorAttribute))]
    internal sealed class GroupSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new GUIUtils.PropertyScope(position, property, ref label))
                GUIUtils.DrawGroupSelectionField(position, property, (property.serializedObject.targetObject as Component).gameObject);
        }
    }

}
