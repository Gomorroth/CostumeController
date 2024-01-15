using gomoru.su.CostumeController.Components.Controls;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCCostumeControl))]
    internal sealed class CCCostumeControlEditor : Editor
    {
        public void OnEnable()
        {
            (serializedObject.targetObject as CCCostumeControl).UpdateItems();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var items = serializedObject.FindProperty(nameof(CCCostumeControl.Items));
            if (items.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(items.isExpanded, "Items"))
            {
                for (int i = 0; i < items.arraySize; i++)
                {
                    var item = items.GetArrayElementAtIndex(i);
                    var rect = EditorGUILayout.GetControlRect();
                    EditorGUI.ObjectField(rect, GUIContent.none, item.FindPropertyRelative(nameof(CCCostumeControl.Item.Object)).objectReferenceValue, typeof(SkinnedMeshRenderer), false);
                    if (item.isExpanded = EditorGUI.Foldout(rect, item.isExpanded, GUIContent.none))
                    {
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        EditorGUILayout.PropertyField(item.FindPropertyRelative(nameof(CCCostumeControl.Item.Include)));
                        EditorGUILayout.PropertyField(item.FindPropertyRelative(nameof(CCCostumeControl.Item.Active)));
                        EditorGUILayout.PropertyField(item.FindPropertyRelative(nameof(CCCostumeControl.Item.Save)));
                        EditorGUILayout.PropertyField(item.FindPropertyRelative(nameof(CCCostumeControl.Item.Sync)));
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
