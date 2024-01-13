using gomoru.su.CostumeController.Components.Controls;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController.Inspector
{
    internal abstract class OptionalControllerEditorBase<TControl, TDrawer> : Editor where TControl : OptionalControl where TDrawer : OptionalControlDrawer<TControl, TDrawer>, new()
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ControllerBase.IsActiveByDefault)), "Active".ToGUIContent());
            EditorGUILayout.Space();

            var name = serializedObject.FindProperty(nameof(ControllerBase.Name));
            EditorGUILayout.PropertyField(name);
            if (string.IsNullOrEmpty(name.stringValue))
                name.stringValue = serializedObject.targetObject.name;
            GUIUtils.DrawControlWithSelectionButton(EditorGUILayout.GetControlRect(), serializedObject.FindProperty(nameof(ControllerBase.Group)), (position, property) => EditorGUI.PropertyField(position, property));

            EditorGUILayout.Space();

            OnInnerGUI();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ControllerBase.OptionalControls)));
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnInnerGUI()
        {
            var instance = Singleton<TDrawer>.Instance;
            var position = EditorGUILayout.GetControlRect(false, (OptionalControlDrawer.Margin + EditorGUIUtility.singleLineHeight) * instance.GetPropertyCountWithoutTargetObjectField(SerializedControl, GUIContent.none));
            instance.DrawWithoutTargetObjectField(position, SerializedControl);
        }

        private SerializedProperty serializedControl;
        protected SerializedProperty SerializedControl
        {
            get
            {
                if (serializedControl == null)
                    serializedControl = serializedObject.FindProperty(nameof(OptionalControllerBase<TControl>.Control));
                return serializedControl;
            }
        }
    }
}
