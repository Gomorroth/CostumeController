using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class ControllerBase : CCBaseComponent, IControl
    {
        public bool IsActiveByDefault;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;
    }
}