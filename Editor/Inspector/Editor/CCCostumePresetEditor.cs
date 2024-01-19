using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System;

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

                var items = component.gameObject.GetRootObject().GetComponentsInChildren<IParameterNamesProvider>().SelectMany(x => x.GetParameterNames()).Distinct().Select(x => new GUIContent($"{x.Group}/{x.Name}", AssetPreview.GetMiniTypeThumbnail(typeof(GameObject)))).ToArray();
                var idx = items.AsSpan().IndexOf(x => x.text == property.stringValue);
                
                EditorGUI.BeginChangeCheck();
                idx = EditorGUI.Popup(position, GUIContent.none, idx, items, EditorStyles.objectField);
                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = items[idx].text;
                }

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
