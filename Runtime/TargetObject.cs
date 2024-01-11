using System;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public sealed class TargetObject
    {
        public string Path;
        public bool IsAbsolute;

        public GameObject GetTargetGameObject(GameObject container)
        {
            if (container == null)
                return null;
            var root = IsAbsolute ? container.GetRootObject() : container;
            return root.Find(Path);
        }
    }
}
