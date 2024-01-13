using System.Linq;
using gomoru.su.CostumeController.Controls;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(MaterialControl))]
    internal sealed class MaterialControlDrawer : OptionalControlDrawer<MaterialControl, MaterialControlDrawer>
    {
        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => ShowTargetObjectField ? 4 : 3;

        private GUIContent[] labelCache;
        private GameObject previousTargetObject;

        public override void Draw(Rect position, SerializedProperty property)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(OptionalControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (ShowTargetObjectField)
            {
                DrawTargetObject(position, targetObjectProp, rootObj, typeof(SkinnedMeshRenderer));
                NewLine(ref position);
            }

            var targetObj = (targetObjectProp.boxedValue as TargetObject).GetTargetGameObject(rootObj);
            var targetSmr = targetObj == null ? null : targetObj.GetComponent<SkinnedMeshRenderer>();
            var targetMaterials = targetSmr == null ? null : targetSmr.sharedMaterials;

            var indexProp = property.FindPropertyRelative(nameof(MaterialControl.Index));

            if (targetObj == null)
            {
                EditorGUI.PropertyField(position, indexProp, "Target Material Index".ToGUIContent());
            }
            else
            {
                if (previousTargetObject == null || previousTargetObject != targetObj)
                {
                    previousTargetObject = targetObj;
                    labelCache = new GUIContent[targetMaterials.Length];
                    for (int i = 0; i < targetMaterials.Length; i++)
                    {
                        labelCache[i] = new GUIContent(targetMaterials[i].name ?? "");
                    }
                    indexProp.intValue = Mathf.Clamp(indexProp.intValue, 0, targetMaterials.Length - 1);
                }
                GUIUtils.ChangeCheck(indexProp, (int value) => EditorGUI.Popup(position, "Target Material".ToGUIContent(), value, labelCache, EditorStyles.popup));
            }


            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Enabled)));
        }
    }

    [CustomPropertyDrawer(typeof(ColorControl))]
    internal sealed class ColorControlDrawer : OptionalControlDrawer<ColorControl, ColorControlDrawer>
    {
        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => ShowTargetObjectField ? 4 : 3;

        public override void Draw(Rect position, SerializedProperty property)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(OptionalControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (ShowTargetObjectField)
            {
                DrawTargetObject(position, targetObjectProp, rootObj, typeof(Renderer));
                NewLine(ref position);
            }

            var targetObj = (targetObjectProp.boxedValue as TargetObject).GetTargetGameObject(rootObj);
            var targetRenderer = targetObj == null ? null : targetObj.GetComponent<Renderer>();
            var targetMaterials = targetRenderer == null ? null : targetRenderer.sharedMaterials;

            var propertyNameProp = property.FindPropertyRelative(nameof(ColorControl.PropertyName));

            if (GUIUtils.DrawControlWithSelectionButton(position, propertyNameProp, 
                (position, property) => EditorGUI.PropertyField(position, property), 
                targetMaterials.Where(x => x != null).Any()))
            {
                var items = targetMaterials.SelectMany(x => x.GetPropertyNames(MaterialPropertyType.Vector)).Select(x => ObjectNames.NicifyVariableName(x)).OrderBy(x => x).ToArray();
                SelectionWindow.Show(items, index => { propertyNameProp.stringValue = items[index]; propertyNameProp.serializedObject.ApplyModifiedProperties(); });
            }
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Disabled)));
            NewLine(ref position);

            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(ControlBase<object>.Enabled)));
        }
    }
}
