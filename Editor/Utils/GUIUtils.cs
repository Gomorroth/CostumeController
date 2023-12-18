using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace gomoru.su.CostumeController
{
    internal static class GUIUtils
    {
        private static GUIContent _tempContent; 

        public static GUIContent ToGUIContent(this string text, string toolTip = null, Texture image = null)
        {
            var content = _tempContent ??= new GUIContent();
            content.text = text;
            content.tooltip = toolTip;
            content.image = image;
            return content;
        }

        public static (float Width, float Height) CalcSize(string text, GUIStyle style) => style.CalcSize(text.ToGUIContent()).AsTuple();
    }
}
