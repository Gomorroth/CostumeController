using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Costume Control")]
    [ExecuteInEditMode]
    public sealed class CCCostumeControl : CCBaseComponent, IControl, IControlTargetProvider
    {
        public Item[] Items;

        [SerializeField]
        private bool initialized;

        public void Start() => Initialize();
        public void Reset() => Initialize();

        private void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            var list = RuntimeUtils.SharedList<SkinnedMeshRenderer>.Instance;
            GetComponentsInChildren(true, list);
            var renderers = list.AsSpan();
            var items = (Items = new Item[renderers.Length]).AsSpan();

            for (int i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                var renderer = renderers[i];

                item = new() { Active = renderer.gameObject.activeInHierarchy, Object = renderer.gameObject, Save = true, Sync = true };
            }
        }

        public void GetControlTargets(ref ValueList<GameObject> destination)
        {
        }

        [Serializable]
        public sealed class Item
        {
            public GameObject Object;

            public bool Active;
            public bool Save;
            public bool Sync;

            [SerializeReference]
            public List<OptionalControl> OptionalControls = new();
        }
    }
}