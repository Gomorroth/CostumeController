using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Costume Control")]
    public sealed class CCCostumeControl : CCBaseComponent, IControl, IControlTargetProvider, IHierarchyChangedCallback
    {
        public int HashCode;

        public Item[] Items;

        public void GetControlTargets(ref ValueList<GameObject> destination)
        {
        }

        public void OnHierarchyChanged() => UpdateItems();

        public void UpdateItems()
        {
            var hash = GetHashCode(this.gameObject, out var renderers).ToHashCode();
            if (HashCode == hash)
                return;

            HashCode = hash;

            var items = Items.AsSpan();
            var newItems = (Items = new Item[renderers.Length]).AsSpan();

            for(int i = 0; i < newItems.Length; i++)
            {
                ref var item = ref newItems[i];
                var renderer = renderers[i];
                var obj = renderer.gameObject;

                if (items.FirstOrDefault(x => x.Object == obj) is { } origin)
                {
                    item = origin;
                }
                else
                {
                    item.Object = obj;
                }
            }

            this.MarkDirty();
        }

        private static HashCode GetHashCode(GameObject root, out Span<SkinnedMeshRenderer> renderers)
        {
            var list = RuntimeUtils.SharedList<SkinnedMeshRenderer>.Instance;
            root.GetComponentsInChildren(true, list);
            renderers = list.AsSpan();
            var hash = new HashCode();
            foreach ( var renderer in renderers)
            {
                hash.Add(renderer);
            }
            return hash;
        }

        [Serializable]
        public struct Item
        {
            public GameObject Object;

            public bool Include;
            
            public bool Active;

            public bool Save;

            public bool Sync;

            [SerializeReference]
            public List<OptionalControl> OptionalControls;

            public static Item Default => new() { Include = true, Active = true, Save = true, Sync = true, OptionalControls = new() };
        }
    }
}