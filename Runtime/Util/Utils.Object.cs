using System;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    partial class Utils
    {
        [ThreadStatic]
        private static string[] _relativePathBuffer;

        public static string GetRelativePath(this GameObject gameObject, GameObject relativeTo)
        {
            if (relativeTo == gameObject) return "";

            var buffer = _relativePathBuffer ??= new string[256];
            int i = buffer.Length - 1;

            while (gameObject != relativeTo && gameObject != null)
            {
                if (i < 0)
                {
                    var newBuffer = new string[buffer.Length * 2];
                    buffer.CopyTo(newBuffer.AsSpan(buffer.Length));
                    i += buffer.Length;
                    _relativePathBuffer = buffer = newBuffer;
                }
                buffer[i--] = gameObject.name;
                gameObject = gameObject.transform.parent?.gameObject;
            }

            if (gameObject == null && relativeTo != null) return null;

            i++;

            return string.Join("/", buffer, i, buffer.Length - i);
        }

        public static bool IsChildren(this GameObject root, GameObject target, System.IO.SearchOption searchOption = System.IO.SearchOption.AllDirectories)
        {
            if (searchOption == System.IO.SearchOption.TopDirectoryOnly)
                return target?.transform?.parent == root.transform;

            var t = target?.transform?.parent;
            while (t != null)
            {
                if (t.gameObject == root)
                    return true;
                t = t.parent;
            }
            return false;
        }

        public static GameObject GetRootObject(this GameObject gameObject)
        {
            if (gameObject == null)
                return null;

            var tr = gameObject.transform;
            while (tr.parent != null)
            {
                tr = tr.parent;
            }
            return tr.gameObject;
        }

        public static GameObject Find(this GameObject obj, string path)
        {
            return obj?.transform.Find(path)?.gameObject;
        }

    }
}
