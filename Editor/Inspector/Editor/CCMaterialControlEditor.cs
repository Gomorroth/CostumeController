using gomoru.su.CostumeController.Components.Controls;
using gomoru.su.CostumeController.Controls;
using UnityEditor;

namespace gomoru.su.CostumeController.Inspector
{
    [CustomEditor(typeof(CCMaterialControl))]
    internal sealed class CCMaterialControlEditor : OptionalControllerEditorBase<MaterialControl, MaterialControlDrawer> { }
}
