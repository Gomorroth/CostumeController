using System.Collections;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase<TControl> : ObjectControllerBase where TControl : OptionalControl
    {
        public TControl Control;
    }
}