using gomoru.su.CostumeController.Attributes;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase<TControl> : ObjectControllerBase where TControl : OptionalControl
    {
        [ForceAbsolutePath]
        public TControl Control;
    }
}