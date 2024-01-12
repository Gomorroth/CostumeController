﻿using gomoru.su.CostumeController.Controls;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(BlendshapeControl))]
    internal sealed class BlendshapeControlDrawer : OptionalControlDrawer
    {
        public static BlendshapeControlDrawer Default => Singleton<BlendshapeControlDrawer>.Instance;

        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(ref position, property, label);
            if (!property.isExpanded || property.boxedValue is not BlendshapeControl)
                return;

            Draw(position, property);
        }

        public override void Draw(Rect position, SerializedProperty property, bool showTargetObject = true)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(BlendshapeControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (showTargetObject)
            {
                DrawTargetObject(position, targetObjectProp, rootObj, typeof(SkinnedMeshRenderer));
                NewLine(ref position);
            }

            var targetObj = (targetObjectProp.boxedValue as TargetObject).GetTargetGameObject(rootObj);
            var targetSmr = targetObj == null ? null : targetObj.GetComponent<SkinnedMeshRenderer>();
            var targetMesh = targetSmr == null ? null : targetSmr.sharedMesh;

            var blendshapeProp = property.FindPropertyRelative(nameof(BlendshapeControl.Blendshape));

            if (GUIUtils.DrawControlWithSelectionButton(position, blendshapeProp, 
                static (position, property) => EditorGUI.PropertyField(position, property), 
                targetMesh != null && targetMesh.blendShapeCount > 0))
            {
                SelectionWindow.Show(Enumerable.Range(0, targetMesh.blendShapeCount).Select(i => targetMesh.GetBlendShapeName(i)), i =>
                {
                    blendshapeProp.stringValue = targetMesh.GetBlendShapeName(i);
                    blendshapeProp.serializedObject.ApplyModifiedProperties();
                });
            }

            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Enabled)));
        }
    }

}