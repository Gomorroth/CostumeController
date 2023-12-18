using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    static partial class Utils
    {
        public static (float X, float Y) AsTuple(this Vector2 vector) => (vector.x, vector.y);
    }
}
