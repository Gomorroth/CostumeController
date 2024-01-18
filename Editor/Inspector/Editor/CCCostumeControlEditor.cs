using gomoru.su.CostumeController.Components.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
                },
                onCanAddCallback = static x =>
                {
                    var list = RuntimeUtils.SharedList<SkinnedMeshRenderer>.Instance;
                    (x.serializedProperty.serializedObject.targetObject as Component).GetComponentsInChildren(true, list);
                    return x.count < list.Count;
                },
            };

            float GetElementHeight(SerializedProperty property)
            {
                return EditorGUIUtility.singleLineHeight + (!property.isExpanded ? 0 : GUIUtils.GetHeight(property.FindPropertyRelative(nameof(CCCostumeControl.Item.OptionalControls)), GUIContent.none, true));
            }
            
            void OnGUI(Rect position, SerializedProperty property)
            {
                var component = target as CCCostumeControl;
                position.height = EditorGUIUtility.singleLineHeight;

                var objProp = property.FindPropertyRelative(nameof(CCCostumeControl.Item.Object));

                var line1itemField = position;
                float margin = 10;
                line1itemField.x += margin;
                line1itemField.width -= margin;

                if (objProp.objectReferenceValue == null)
                {
                    var list = RuntimeUtils.SharedList<SkinnedMeshRenderer>.Instance;
                    (target as Component).GetComponentsInChildren(true, list);
                    var items = list.Where(x => !component.Items.Any(y => y.Object == x.gameObject)).Select(x => x.gameObject);
                    var labels = Enumerable.Repeat("None", 1).Concat(items.Select(x => x.name)).ToArray();

                    EditorGUI.BeginChangeCheck();
                    var idx = EditorGUI.Popup(line1itemField, 0, labels);
                    if (EditorGUI.EndChangeCheck())
                    {
                        objProp.objectReferenceValue = items.ElementAt(idx - 1);
                    }

                    return;
                }

                EditorGUI.ObjectField(line1itemField, GUIContent.none, objProp.objectReferenceValue, typeof(SkinnedMeshRenderer), false);
                if (property.isExpanded = EditorGUI.Foldout(line1itemField, property.isExpanded, GUIContent.none))
                {
                    EditorGUI.indentLevel++;
                    position.y += EditorGUIUtility.singleLineHeight;

                    if (foldoutHeaderReplacementStyle == null)
                    {
                        var style = foldoutHeaderReplacementStyle = new(EditorStyles.foldout);
                        style.focused.textColor = EditorStyles.label.focused.textColor;
                    }

                    using (new ReplaceFoldoutHeaderStyleScope(foldoutHeaderReplacementStyle))
                        EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(CCCostumeControl.Item.OptionalControls)));

                    EditorGUI.indentLevel--;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CCCostumeControl.Group)));
            //GUIUtils.DrawGroupSelectionField(EditorGUILayout.GetControlRect(), serializedObject.FindProperty(nameof(CCCostumeControl.Group)), (target as Component).gameObject);
            itemList.DoLayoutList();


            serializedObject.ApplyModifiedProperties();
        }

        private static GUIStyle foldoutHeaderReplacementStyle;

        private readonly ref struct ReplaceFoldoutHeaderStyleScope
        {
            private readonly GUIStyle original;

            public ReplaceFoldoutHeaderStyleScope(GUIStyle replaceTo) => ReplaceFoldoutHeaderStyle(replaceTo, out original);

            public void Dispose() => ReplaceFoldoutHeaderStyle(original, out _);

            private delegate void ReplaceFoldoutHeaderStyleDelegate(GUIStyle replaceTo, out GUIStyle original);

            private static readonly ReplaceFoldoutHeaderStyleDelegate ReplaceFoldoutHeaderStyleMethod;

            private static void ReplaceFoldoutHeaderStyle(GUIStyle replaceTo, out GUIStyle original) 
                => ReplaceFoldoutHeaderStyleMethod(replaceTo, out original);

            static ReplaceFoldoutHeaderStyleScope()
            {
                var method = new DynamicMethod("", null, new[] { typeof(GUIStyle), typeof(GUIStyle).MakeByRefType() }, typeof(EditorStyles), true);
                var il = method.GetILGenerator();

                var currentStylesField = typeof(EditorStyles).GetField("s_Current", BindingFlags.NonPublic | BindingFlags.Static);
                var foldoutHeaderField = typeof(EditorStyles).GetField("m_FoldoutHeader", BindingFlags.NonPublic | BindingFlags.Instance);

                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldsfld, currentStylesField);
                il.Emit(OpCodes.Ldfld, foldoutHeaderField);
                il.Emit(OpCodes.Stind_Ref);
                il.Emit(OpCodes.Ldsfld, currentStylesField);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Stfld, foldoutHeaderField);
                il.Emit(OpCodes.Ret);

                ReplaceFoldoutHeaderStyleMethod = method.CreateDelegate(typeof(ReplaceFoldoutHeaderStyleDelegate)) as ReplaceFoldoutHeaderStyleDelegate;
            }
        }
    }
}
