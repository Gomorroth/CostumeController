using gomoru.su.CostumeController.Components.Controls;
using gomoru.su.CostumeController.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCBlendshapeControl))]
    internal sealed class CCBlendshapeControlEditor : OptionalControllerEditorBase<BlendshapeControl>
    {
        protected override void OnInnerGUI()
        {
            var position = EditorGUILayout.GetControlRect(false, (OptionalControlDrawer.Margin + EditorGUIUtility.singleLineHeight) * 3);
            BlendshapeControlDrawer.Default.Draw(position, SerializedControl, false);
        }
    }
}
