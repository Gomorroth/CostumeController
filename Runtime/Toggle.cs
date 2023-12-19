using System;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public struct Toggle<T>
    {
        public T Value;
        public bool Enable;
    }
}
