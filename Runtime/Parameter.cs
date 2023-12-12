using System;
using System.Collections.Generic;
using System.Text;

namespace gomoru.su.CostumeController
{
    [Serializable]
    internal struct Parameter<T>
    {
        public T Value;
        public bool IsLocal;
        public bool IsSaved;
    }
}
