using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gomoru.su.CostumeController
{
    internal static partial class EditorUtils
    {
        public static (string Group, string Name)[] GetParameterNames<TProvider>(this TProvider provider) where TProvider : IParameterNamesProvider
        {
            var list = ValueList<(string, string)>.Create(32);
            try
            {
                provider.GetParameterNames(ref list);
                return list.Span.ToArray();
            }
            finally
            {
                list.Dispose();
            }
        }
    }
}
