using gomoru.su.CostumeController.Attributes;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase<TControl> : ControllerBase where TControl : OptionalControl
    {
        public TControl Control;
    }
}