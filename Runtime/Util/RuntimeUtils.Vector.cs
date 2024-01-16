using UnityEngine;

namespace gomoru.su.CostumeController
{
    static partial class RuntimeUtils
    {
        public static (float X, float Y) AsTuple(this Vector2 vector) => (vector.x, vector.y);

        public static void Deconstruct(this Vector2 vector, out float x, out float y) => (x, y) = (vector.x, vector.y);
    }
}
