using UnityEngine;

namespace gomoru.su.CostumeController.Controls
{
    public abstract class ControlBase<T> : OptionalControl
    {
        public Toggle<T> Enabled = new(GetEnableValue());
        public Toggle<T> Disabled;

        private static T GetEnableValue()
        {
            if (typeof(T) == typeof(int))
                return (T)(object)1;
            if (typeof(T) == typeof(float))
                return (T)(object)1f;
            if (typeof(T) == typeof(bool))
                return (T)(object)true;
            if (typeof(T) == typeof(Color))
                return (T)(object)Color.white;
            if (typeof(T) == typeof(Vector4))
                return (T)(object)Vector4.one;
            return default;
        }
    }
}
