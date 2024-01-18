using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public sealed class ObjectPath
    {
        public string Path = RelativePathPrefix;

        private const string AbsolutePathPrefix = "/";
        private const string RelativePathPrefix = "./";

        public PathMode PathMode => IsAbsolutePath(Path) ? PathMode.Absolute : PathMode.Relative;

        public string ChangePathMode(GameObject container, PathMode mode)
        {
            if (PathMode != mode)
            {
                return GetTargetPath(container, GetObject(container), mode);
            }
            return Path;
        }

        public static string GetTargetPath(GameObject container, GameObject target, PathMode pathMode = PathMode.Relative)
        {
            bool isAbsolute = pathMode is PathMode.Absolute;
            var prefix = isAbsolute ? AbsolutePathPrefix : RelativePathPrefix;
            var root = isAbsolute ? container.GetRootObject() : container;
            if (root == target)
            {
                return prefix;
            }

            var path = target.GetRelativePath(root);
            if (path is null)
                return null;
            return prefix + path; 
        }

        public GameObject GetObject(GameObject container)
        {
            if (container == null || string.IsNullOrEmpty(Path))
                return null;

            if (Path is AbsolutePathPrefix)
                return container.GetRootObject();
            else if (Path is RelativePathPrefix)
                return container;

            bool isAbsolute = IsAbsolutePath(Path);
            var root = isAbsolute ? container.GetRootObject() : container;
            return root.Find(Path[(isAbsolute ? 1 : 2)..]);
        }

        private static bool IsAbsolutePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            return path[0] == '/';
        }

        public ObjectPath() { }

        public ObjectPath(string path) => Path = path;

        public ObjectPath(GameObject container, GameObject target) => Path = GetTargetPath(container, target, container.IsChildren(target) ? PathMode.Relative : PathMode.Absolute);

        public ObjectPath(GameObject container, GameObject target, PathMode mode) => Path = GetTargetPath(container, target, mode);
    }
}
