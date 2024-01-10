using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using gomoru.su.CostumeController;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


[assembly: ExportsPlugin(typeof(CostumeController))]

namespace gomoru.su.CostumeController
{
    [InitializeOnLoad]
    public sealed partial class CostumeController : Plugin<CostumeController>
    {
        protected override void Configure()
        {
            InPhase(BuildPhase.Generating)
                .WithRequiredExtension(typeof(CostumeControllerContext), sequence =>
                {
                    sequence.Run(N.Instance);
                });
        }

        public sealed class CostumeControllerContext : IExtensionContext
        {
            public void OnActivate(BuildContext context)
            {
            }

            public void OnDeactivate(BuildContext context)
            {
                foreach(var x in context.AvatarRootObject.FindComponentsInChildren<CCBaseComponent>())
                {
                    Object.DestroyImmediate(x);
                }
            }
        }

        public sealed class N : Pass<N>
        {
            protected override void Execute(BuildContext context)
            {
                context.Extension<CostumeControllerContext>();
            }
        }
    }
}
