using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class ControllerBase : CCBaseComponent, IControl, IControlTargetProvider
    {
        public string Name;
        public string Group;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;

        public void GetControlTargets(ref ValueList<GameObject> destination)
        {
            destination.Add(gameObject);
        }
    }
}