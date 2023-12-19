using System;

namespace gomoru.su.CostumeController.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    internal sealed class ForceAbsolutePathAttribute : Attribute { }
}
