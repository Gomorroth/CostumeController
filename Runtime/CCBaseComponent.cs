using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace gomoru.su.CostumeController
{
    [ExecuteInEditMode]
    public abstract class CCBaseComponent : MonoBehaviour, IEditorOnly
    {
        [SerializeField]
        protected bool Initialized;

        private void Start() => Initialize();

        private void Reset() => Initialize();

        private void Initialize()
        {
            if (Initialized)
                return;
            Initialized = true;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }
    }
}
