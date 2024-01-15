#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace gomoru.su.CostumeController
{
    static partial class RuntimeUtils
    {
        public static T MarkDirty<T>(this T obj) where T : Object
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
            return obj;
        }

        public static T Record<T>(this T obj, string name = null) where T : Object
        {
#if UNITY_EDITOR
            Undo.RecordObject(obj, name);
#endif
            return obj;
        }

        public static T AddComponentWithUndo<T>(this GameObject obj) where T : Component
        {
#if UNITY_EDITOR
            return Undo.AddComponent<T>(obj);
#else
            return obj.AddComponent<T>();
#endif
        }
    }
}