using gomoru.su.CostumeController.Components.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    internal abstract class OptionalControllerEditorBase<T> : Editor where T : OptionalControl
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

        protected abstract void OnInnerGUI();

        private SerializedProperty serializedControl;
        protected SerializedProperty SerializedControl
        {
            get
            {
                if (serializedControl == null)
                    serializedControl = serializedObject.FindProperty(nameof(OptionalControllerBase<T>.Control));
                return serializedControl;
            }
        }
    }
}
