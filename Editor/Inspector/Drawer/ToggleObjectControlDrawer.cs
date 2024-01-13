using gomoru.su.CostumeController.Controls;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(ToggleObjectControl))]
    internal sealed class ToggleObjectControlDrawer : OptionalControlDrawer<ToggleObjectControl, ToggleObjectControlDrawer>
    {
        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => 3;

        public override void Draw(Rect position, SerializedProperty property, bool showTargetObject = true)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(OptionalControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (showTargetObject)
            {
                DrawTargetObject(position, targetObjectProp, rootObj, typeof(GameObject));
                NewLine(ref position);
            }

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Enabled)));
        }
    }
}
