using System;
using System.Linq;
using UnityEditor;

namespace gomoru.su.CostumeController
{
    [InitializeOnLoad]
    internal static class OptionalControls
    {
        public static readonly Type[] Types;
        public static readonly string[] Labels;
        static OptionalControls()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && typeof(OptionalControl).IsAssignableFrom(x)).ToArray();

            var labels = new string[types.Length + 1];
            labels[0] = "None";
            for(int i = 1; i < labels.Length; i++)
            {
                int k = i - 1;
                var name = types[k].Name;
                if (name.EndsWith("Control"))
                    name = name[..^"Control".Length];

                labels[i] = ObjectNames.NicifyVariableName(name);
            }

            Types = types;
            Labels = labels;
        }
    }
}
