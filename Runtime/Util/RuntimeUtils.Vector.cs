using UnityEngine;

namespace gomoru.su.CostumeController
{
    static partial class RuntimeUtils
    {
        public static (float X, float Y) AsTuple(this Vector2 vector) => (vector.x, vector.y);
    }
}
