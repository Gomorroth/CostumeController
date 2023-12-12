﻿using System;
using UnityEngine;

namespace gomoru.su.CostumeController.Controls
{
    [Serializable]
    public class ColorControl : ControlBase<Color>
    {
        public string PropertyName;
        public ColorField Include = (ColorField)(-1);
    }
}
