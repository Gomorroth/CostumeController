using System;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public struct Toggle<T>
    {
        public T Value;
        public bool Enable;

        public Toggle(T value, bool enable = false)
        {
            Value = value;
            Enable = enable;
        }
    }
}
