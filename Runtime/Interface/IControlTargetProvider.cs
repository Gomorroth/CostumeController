using System.Collections.Generic;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    public interface IControlTargetProvider
    {
        void GetControlTargets(ref ValueList<GameObject> destination);
    }
}
