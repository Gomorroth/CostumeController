using gomoru.su.CostumeController.Components.Controls;
using gomoru.su.CostumeController.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCBlendshapeControl))]
    internal sealed class CCBlendshapeControlEditor : OptionalControllerEditorBase<BlendshapeControl, BlendshapeControlDrawer>
    {
    }
}
