using System;
using gomoru.su.CostumeController.Attributes;

namespace gomoru.su.CostumeController.Controls
{
    [Serializable]
    [MinMax(nameof(Enabled), min: 0, max: 100)]
    [MinMax(nameof(Disabled), min: 0, max: 100)]
    public class BlendshapeControl : ControlBase<float>
    {
        public string Blendshape;
    }
}
