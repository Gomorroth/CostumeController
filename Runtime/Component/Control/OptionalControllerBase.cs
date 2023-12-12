using System.Collections;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase<TControl> : ObjectControllerBase where TControl : IOptionalControl
    {
        public TControl Control;
    }
}