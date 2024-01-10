using gomoru.su.CostumeController.Controls;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Blendshape Control")]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public sealed partial class CCBlendshapeControl : OptionalControllerBase<BlendshapeControl>
    {
    }

}