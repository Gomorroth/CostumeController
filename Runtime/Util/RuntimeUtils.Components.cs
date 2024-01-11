using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    partial class RuntimeUtils
    {
        public static ReadOnlySpan<TComponent> FindComponents<TComponent>(this Component component) where TComponent : Component
            => component.GetComponents<TComponent>();

        public static ReadOnlySpan<TComponent> FindComponents<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            var list = SharedList<TComponent>.Instance;
            int count = list.Count;
            gameObject.GetComponents(list);
            return list.AsSpan()[count..];
        }

        public static ReadOnlySpan<TComponent> FindComponentsInChildren<TComponent>(this Component component, bool includeInactive = false) where TComponent : Component
            => component.gameObject.FindComponentsInChildren<TComponent>(includeInactive);

        public static ReadOnlySpan<TComponent> FindComponentsInChildren<TComponent>(this GameObject gameObject, bool includeInactive = false) where TComponent : Component
        {
            var list = SharedList<TComponent>.Instance;
            int count = list.Count;
            gameObject.GetComponentsInChildren(includeInactive, list);
            return list.AsSpan()[count..];
        }
    }
}
