using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Costume Control")]
    public sealed class CCCostumeControl : CCBaseComponent, IControl, IControlTargetProvider
    {
        public int HashCode;

        public List<GameObject> Items;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;

        public void GetControlTargets(ref ValueList<GameObject> destination)
        {
            destination.AddRange(Items.AsSpan());
        }
    }
}