using gomoru.su.CostumeController.Components.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCCostumeControl))]
    internal sealed class CCCostumeControlEditor : Editor
    {
        private ReorderableList itemList;

        private void OnEnable()
        {
            InitializeItemList();
        }

        private void InitializeItemList()
        {
            itemList = new ReorderableList(serializedObject, serializedObject.FindProperty(nameof(CCCostumeControl.Items)))
            {
                headerHeight = 0,
                elementHeightCallback = (index) => GetElementHeight(itemList.serializedProperty.GetArrayElementAtIndex(index)),
                drawElementCallback = (position, index, active, focused) => OnGUI(position, itemList.serializedProperty.GetArrayElementAtIndex(index)),
                onAddCallback = static list =>
                {
                    using var x = new UndoScope("Add item");

                    int index = list.serializedProperty.arraySize;
                    list.serializedProperty.InsertArrayElementAtIndex(index);
                    var newItem = list.serializedProperty.GetArrayElementAtIndex(index);
                    newItem.FindPropertyRelative(nameof(CCCostumeControl.Item.Object)).objectReferenceValue = null;
                }
            };

            float GetElementHeight(SerializedProperty property)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            
            void OnGUI(Rect position, SerializedProperty property)
            {
                var component = target as CCCostumeControl;
                position.height = EditorGUIUtility.singleLineHeight;

                var objProp = property.FindPropertyRelative(nameof(CCCostumeControl.Item.Object));

                {
                    EditorGUI.BeginChangeCheck();
                    var selected = EditorGUI.ObjectField(position, GUIContent.none, objProp.objectReferenceValue, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selected == null)
                        {
                            objProp.objectReferenceValue = null;
                        }
                        else if ((target as Component).gameObject.IsChildren(selected.gameObject) 
                            && !component.Items.Any(x => x.Object == selected.gameObject))
                        {
                            objProp.objectReferenceValue = selected.gameObject;
                        }
                    }
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
