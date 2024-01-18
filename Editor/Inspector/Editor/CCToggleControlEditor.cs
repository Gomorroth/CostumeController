using gomoru.su.CostumeController.Components.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCToggleControl))]
    internal sealed class CCToggleControlEditor : OptionalControllerEditorBase<ToggleObjectControl, ToggleObjectControlDrawer> { }
}
