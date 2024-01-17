using gomoru.su.CostumeController.Controls;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(BlendshapeControl))]
    internal sealed class BlendshapeControlDrawer : OptionalControlDrawer<BlendshapeControl, BlendshapeControlDrawer>
    {
        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => ShowTargetObjectField ? 4 : 3;

        public override void Draw(Rect position, SerializedProperty property)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(BlendshapeControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (ShowTargetObjectField)
            {
                GUIUtils.DrawTargetObject(position, targetObjectProp, rootObj, typeof(SkinnedMeshRenderer));
                NewLine(ref position);
            }

            var targetObj = (targetObjectProp.boxedValue as TargetObject).GetObject(rootObj);
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
                }, "Select Blendshape");
            }

            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(BlendshapeControl.Enabled)));
        }
    }
}
