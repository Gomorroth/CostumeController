using gomoru.su.CostumeController.Attributes;
using System;

namespace gomoru.su.CostumeController
{
    [Serializable]
    public abstract class OptionalControl
    {
        public TargetObject Target;
    }

    [Serializable]
    [ForceAbsolutePath]
    public abstract class AbsolutePathOptionalControl : OptionalControl { }
}
