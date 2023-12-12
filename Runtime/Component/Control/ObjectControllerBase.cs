using System.Collections.Generic;
using gomoru.su.CostumeController.Controls;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    public abstract class ObjectControllerBase : CCComponentBase, IControl
    {
        public bool IsActiveByDefault;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;
    }
}