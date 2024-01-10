using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class OptionalControllerBase : CCBaseComponent, IControl
    {
        public bool IsActiveByDefault;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;
    }
}