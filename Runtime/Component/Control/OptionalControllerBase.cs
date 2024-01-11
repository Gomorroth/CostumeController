using gomoru.su.CostumeController.Attributes;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase<TControl> : ControllerBase, IControlTargetProvider where TControl : OptionalControl
    {
        public TControl Control;

        public void GetControlTargets(ref ValueList<GameObject> destination)
        {
            destination.Add(gameObject);
        }
    }
}