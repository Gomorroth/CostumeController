using System;
using System.Collections.Generic;
using System.Text;
using gomoru.su.CostumeController.Components.Controls;
using gomoru.su.CostumeController.Controls;
using gomoru.su.CostumeController.SourceGenerator;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CostumeController.Inspector
{
    internal abstract class OptionalControllerEditorBase<T> : Editor where T : OptionalControl
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ControllerBase.IsActiveByDefault)));
            OnGUIInternal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ControllerBase.OptionalControls)));
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void OnGUIInternal();

        private SerializedProperty serializedControl;
        protected SerializedProperty SerializedControl
        {
            get
            {
                if (serializedControl == null)
                    serializedControl = serializedObject.FindProperty(nameof(OptionalControllerBase<T>.Control));
                return serializedControl;
            }
        }
    }

    [CustomEditor(typeof(CCBlendshapeControl))]
    internal sealed class CCBlendshapeControlEditor : OptionalControllerEditorBase<BlendshapeControl>
    {
        protected override void OnGUIInternal()
        {
            var position = EditorGUILayout.GetControlRect(false, (OptionalControlDrawer.Margin + EditorGUIUtility.singleLineHeight) * 3);
            BlendshapeControlDrawer.Default.Draw(position, SerializedControl, false);

        }
    }
}
