using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    [ExecuteInEditMode]
    public abstract class ControllerBase : CCBaseComponent, IControl, IControlTargetProvider, IParameterNamesProvider
    {
        public string Name;

        [GroupSelector]
        public string Group;

        [SerializeField]
        private bool initialized;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;

        protected void Start() => Initialize();
        protected void Reset() => Initialize();

        private void Initialize()
        {
            if (initialized)
                return;
            initialized = true;
            Name = gameObject.name;
            Group = gameObject.transform.parent?.name;
        }

        void IControlTargetProvider.GetControlTargets(ref ValueList<GameObject> destination)
        {
            destination.Add(gameObject);
        }

        void IParameterNamesProvider.GetParameterNames(ref ValueList<(string Group, string Name)> list)
        {
            list.Add((Group, Name));
        }
    }
}