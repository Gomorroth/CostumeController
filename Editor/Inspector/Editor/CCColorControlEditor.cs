using gomoru.su.CostumeController.Components.Controls;
using gomoru.su.CostumeController.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCColorControl))]
    internal sealed class CCColorControlEditor : OptionalControllerEditorBase<ColorControl, ColorControlDrawer> { }
}
