#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace gomoru.su.CostumeController
{
    internal readonly ref struct UndoScope
    {
        private readonly int groupId;

        public UndoScope(string name)
        {
#if UNITY_EDITOR
            Undo.SetCurrentGroupName(name);
            groupId = Undo.GetCurrentGroup();
#else
            groupId = 0;
#endif
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            Undo.CollapseUndoOperations(groupId);
#endif
        }
    }
}
