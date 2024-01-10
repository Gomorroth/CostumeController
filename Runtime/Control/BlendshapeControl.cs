using System;
using System.Reflection;
using gomoru.su.CostumeController.Attributes;
using gomoru.su.CostumeController.SourceGenerator;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController.Controls
{
    [Serializable]
    [MinMax(nameof(Enabled), min: 0, max: 100)]
    [MinMax(nameof(Disabled), min: 0, max: 100)]
    [SerializedPropertyContext]
    public class BlendshapeControl : ControlBase<float>
    {
        public string Blendshape;
    }
}
