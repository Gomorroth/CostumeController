using System.Runtime.CompilerServices;
using gomoru.su.CostumeController.Controls;
using UnityEditor;
using UnityEngine;

[assembly:InternalsVisibleTo("gomoru.su.costume-controller.runtime")]

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(MaterialControl))]
    internal sealed class MaterialControlDrawer : OptionalControlDrawer
    {
        public static MaterialControlDrawer Default => Singleton<MaterialControlDrawer>.Instance;

        public override float GetPropertyCount(SerializedProperty property, GUIContent label) => 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(ref position, property, label);
            if (!property.isExpanded || property.boxedValue is not MaterialControl)
                return;

            Draw(position, property);
        }

        private GUIContent[] labelCache;
        private GameObject previousTargetObject;

        public override void Draw(Rect position, SerializedProperty property, bool showTargetObject = true)
        {
            var targetObjectProp = property.FindPropertyRelative(nameof(OptionalControl.TargetObject));
            var rootObj = (property.serializedObject.targetObject as Component)?.gameObject;
            position.height = EditorGUIUtility.singleLineHeight;

            if (showTargetObject)
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
}
