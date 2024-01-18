using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace gomoru.su.CostumeController.Inspector
{
    using Target = CCCostumePreset;

    [CustomEditor(typeof(Target))]
    internal sealed class CCCostumePresetEditor : Editor
    {
        private ReorderableList itemList;

        private void OnEnable()
        {
            InitializeItemList();
        }

        private void InitializeItemList()
        {
            string[] items = new[] { "A", "B", "C" };

            itemList = new ReorderableList(serializedObject, serializedObject.FindProperty(nameof(Target.Items)))
            {
                headerHeight = 0,
                elementHeightCallback = (index) => GetElementHeight(itemList.serializedProperty.GetArrayElementAtIndex(index)),
                drawElementCallback = (position, index, active, focused) => OnGUI(position, itemList.serializedProperty.GetArrayElementAtIndex(index)),
            };

            float GetElementHeight(SerializedProperty property)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            void OnGUI(Rect position, SerializedProperty property)
            {
                var component = target as Target;
                position.height = EditorGUIUtility.singleLineHeight;

                GUIUtils.DrawControlWithSelectionButton(position, position => GUI.Button(position, property.displayName.ToGUIContent(image: AssetPreview.GetMiniTypeThumbnail(typeof(GameObject))), EditorStyles.objectField));

            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            itemList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
