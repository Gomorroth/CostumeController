using System;

namespace gomoru.su.CostumeController
{
    [Flags]
    public enum ColorField : int
    {
        R = 1 << 0,
        G = 1 << 1,
        B = 1 << 2,
        A = 1 << 3,
    }
}
