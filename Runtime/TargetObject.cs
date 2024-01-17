using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public sealed class TargetObject
    {
        public string Path;

        public bool IsAbsolutePath => Path.StartsWith("/");

        public static string GetTargetPath(GameObject container, GameObject target, bool isAbsolute)
        {
            var root = isAbsolute ? container.GetRootObject() : container;
            if (root == target)
            {
                return isAbsolute ? "/" : "./";
            }

            var path = target.GetRelativePath(root);
            return (isAbsolute ? "/" : "./") + path; 
        }

        public GameObject GetObject(GameObject container)
        {
            if (container == null || string.IsNullOrEmpty(Path))
                return null;

            if (Path == "/")
                return container.GetRootObject();
            else if (Path == "./")
                return container;

            bool isAbsolute = Path.StartsWith("/");
            var root = isAbsolute ? container.GetRootObject() : container;
            Debug.Log(root.Find(Path[(isAbsolute ? 1 : 2)..]));
            return root.Find(Path[(isAbsolute ? 1 : 2)..]);
        }
    }
}
