namespace gomoru.su.CostumeController.Controls
{
    public abstract class ControlBase<T> : IOptionalControl
    {
        public Toggle<T> Enabled;
        public Toggle<T> Disabled;
    }
}
