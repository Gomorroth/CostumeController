using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Costume Control")]
    [ExecuteInEditMode]
    public sealed class CCCostumeControl : CCBaseComponent, IControl, IControlTargetProvider, IParameterNamesProvider
    {
        [GroupSelector]
        public string Group;

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

            Group = gameObject.name;

            var list = RuntimeUtils.SharedList<SkinnedMeshRenderer>.Instance;
            GetComponentsInChildren(true, list);
            var renderers = list.AsSpan();
            var items = (Items = new Item[renderers.Length]).AsSpan();

            for (int i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                var renderer = renderers[i];

                item = new() {Object = renderer.gameObject, Name = renderer.name };
            }
        }

        void IControlTargetProvider.GetControlTargets(ref ValueList<GameObject> destination)
        {
        }

        void IParameterNamesProvider.GetParameterNames(ref ValueList<(string Group, string Name)> list)
        {
            foreach(var item in Items)
            {
                list.Add((Group, item.Name));
            }
        }

        [Serializable]
        public sealed class Item
        {
            public GameObject Object;
            public string Name;

            [SerializeReference]
            public List<OptionalControl> OptionalControls = new();
        }
    }
}