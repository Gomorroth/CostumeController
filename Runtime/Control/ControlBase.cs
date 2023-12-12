namespace gomoru.su.CostumeController.Controls
{
    public abstract class ControlBase<T> : OptionalControl
    {
        public Toggle<T> Enabled;
        public Toggle<T> Disabled;
    }
}
