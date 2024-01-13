using System;
using gomoru.su.CostumeController.Controls;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(OptionalControl))]
    internal class OptionalControlDrawer : PropertyDrawer
    {
        public const float Margin = 2;

        public virtual bool ShowTargetObjectField { get; set; } = true;

        public virtual float GetPropertyCount(SerializedProperty property, GUIContent label) => 0;

        public float GetPropertyCountWithoutTargetObjectField(SerializedProperty property, GUIContent label)
        {
            bool prev = ShowTargetObjectField;
            ShowTargetObjectField = false;
            var result = GetPropertyCount(property, label);
            ShowTargetObjectField = prev;
            return result;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var x = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
            {
                x += (Margin + EditorGUIUtility.singleLineHeight) * GetPropertyCount(property, label) + 1;
            }

            return x;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) => OnGUI(ref position, property, label);

        public void OnGUI(ref Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            //position.y += Margin;
            var value = property.boxedValue;
            int idx = value is null ? 0 : OptionalControls.Types.IndexOf(value.GetType()) + 1;
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
            
            var pos = position;
            pos.x += Margin;
            pos.width -= Margin;

            if (GUIUtils.ChangeCheck(ref idx, () => EditorGUI.Popup(pos, idx, OptionalControls.Labels)))
            {
                property.boxedValue = idx == 0 ? null : Instanciator.Create(OptionalControls.Types[idx - 1]);
                property.isExpanded = idx != 0;
            }
            EditorGUI.showMixedValue = false;

            if (idx != 0)
            {
                property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            }

            NewLine(ref position);
        }

        public virtual void Draw(Rect position, SerializedProperty property) { }

        public void DrawWithoutTargetObjectField(Rect position, SerializedProperty property)
        {
            bool prev = ShowTargetObjectField;
            ShowTargetObjectField = false;
            Draw(position, property);
            ShowTargetObjectField = prev;
        }

        protected static void NewLine(ref Rect position)
        {
            position.y += EditorGUIUtility.singleLineHeight + Margin;
        }

        public static void DrawTargetObject(Rect position, SerializedProperty property, GameObject container = null, Type filterType = null)
        {
            var fieldRect = position;
            var popupRect = position;
            popupRect.width = GUIUtils.CalcSize("Absolute ", EditorStyles.popup).Width;
            fieldRect.width -= popupRect.width + Margin;
            popupRect.x += fieldRect.width + Margin;


            var target = container == null ? null : (property.boxedValue as TargetObject).GetTargetGameObject(container);
            var pathProp = property.FindPropertyRelative(nameof(TargetObject.Path));
            var isAbsoluteProp = property.FindPropertyRelative(nameof(TargetObject.IsAbsolute));
            if (target != null)
            {
                EditorGUI.BeginChangeCheck();
                var result = EditorGUI.ObjectField(fieldRect, "Target Object", target, filterType ?? typeof(GameObject), true);
                if (EditorGUI.EndChangeCheck())
                {
                    GameObject obj = result switch
                    {
                        GameObject x => x,
                        Component x => x.gameObject,
                        _ => null,
                    };

                    if (obj != null)
                    {
                        if (!container.IsChildren(obj))
                        {
                            isAbsoluteProp.boolValue = true;
                        }
                        pathProp.stringValue = obj.GetRelativePath(isAbsoluteProp.boolValue ? container.GetRootObject() : container);
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(fieldRect, pathProp, "Target Object".ToGUIContent());
            }

            GUIUtils.ChangeCheck<bool>(property.FindPropertyRelative(nameof(TargetObject.IsAbsolute)), x => EditorGUI.Popup(popupRect, x ? 1 : 0, new[] { "Relative", "Absolute" }) == 1);
        }
    }

    internal abstract class OptionalControlDrawer<TControl, TDrawer> : OptionalControlDrawer
        where TControl : OptionalControl
        where TDrawer : OptionalControlDrawer<TControl, TDrawer>, new()
    {
        public static TDrawer Default => Singleton<TDrawer>.Instance;

        protected virtual bool CheckPropertyDrawable(SerializedProperty property)
            => property.isExpanded && property.boxedValue is TControl;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(ref position, property, label);
            if (!CheckPropertyDrawable(property))
                return;

            Draw(position, property);
        }
    }
}
