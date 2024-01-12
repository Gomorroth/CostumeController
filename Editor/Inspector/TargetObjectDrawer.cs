using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [CustomPropertyDrawer(typeof(TargetObject))]
    internal sealed class TargetObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var scope = new GUIUtils.PropertyScope(position, property, ref label);

            var path = property.FindPropertyRelative(nameof(TargetObject.Path));
            var isAbsolute = property.FindPropertyRelative(nameof(TargetObject.IsAbsolute));

            if (property.serializedObject.targetObject is not Component component)
            {
                EditorGUI.PropertyField(position, path);
                return;
            }

            bool forceAbsolute = false;

            var objRect = position;
            var toggleRect = position;
            toggleRect.width = GUIUtils.CalcSize("Absolute", EditorStyles.toggle).Width;
            objRect.width -= toggleRect.width + 8;
            toggleRect.x += objRect.width + 4;

            var obj = component.gameObject;
            if (isAbsolute.boolValue)
                obj = obj.GetRootObject();

            var target = obj.Find(path.stringValue);
            if (target != null)
            {
                bool changed;
                using (new GUIUtils.ChangeCheckScope(out changed))
                    target = EditorGUI.ObjectField(objRect, target, typeof(GameObject), true) as GameObject;

                if (changed)
                {
                    var root = component.gameObject;
                    if (!root.IsChildren(target))
                    {
                        isAbsolute.boolValue = true;
                        root = root.GetRootObject();
                    }
                    if (target.GetRelativePath(root) is { } newPath)
                    {
                        path.stringValue = newPath;
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(objRect, path);
            }

            if (!forceAbsolute)

            EditorGUI.ToggleLeft(toggleRect, "Absolute", isAbsolute.boolValue);
        }
    }

}
