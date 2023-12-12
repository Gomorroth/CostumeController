using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace gomoru.su.CostumeController.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
    internal sealed class MinMaxAttribute : Attribute
    {
        public MinMaxAttribute(string target, float min, float max) => (Target, Min, Max) = (target, min, max);

        public string Target { get; }

        public float Min { get; }

        public float Max { get; }
    }
}
