using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController.Components.Controls
{
    [AddComponentMenu("Costume Controller/Control/CC Costume Control")]
    public sealed class CCCostumeControl : CCComponentBase, IControl 
    {
        public int HashCode;

        public List<GameObject> Items;

        [SerializeReference]
        public List<OptionalControl> OptionalControls;
    }
}