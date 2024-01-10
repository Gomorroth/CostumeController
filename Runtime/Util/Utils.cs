using System.Collections.Generic;

namespace gomoru.su.CostumeController
{
    internal static partial class Utils
    {
        public static class SharedList<T>
        {
            private static readonly List<T> instance = new(128);
            public static List<T> Instance
            {
                get
                {
                    if (instance.Count > instance.Capacity * 0.7f)
                        instance.Clear();
                    return instance;
                }
            }
        

        }
    }
}
