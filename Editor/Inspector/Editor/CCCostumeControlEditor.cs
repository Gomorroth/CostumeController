using gomoru.su.CostumeController.Components.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCCostumeControl))]
    internal sealed class CCCostumeControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CCCostumeControl.OptionalControls)));
        }
    }
}
