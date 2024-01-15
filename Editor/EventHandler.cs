using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    [InitializeOnLoad]
    internal static class EventHandler
    {
        static EventHandler()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            foreach (var obj in Object.FindObjectsByType(typeof(Component), FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (obj is IHierarchyChangedCallback callback)
                {
                    callback.OnHierarchyChanged();
                }
            }
        }
    }
}
