using UnityEngine;
using UnityEditor;

namespace FlowKit.Editor
{
    public static class FKMenuItems
    {
        [MenuItem("GameObject/FlowKit/FlowKit Controller", false, 0)]
        public static void CreateFKController(MenuCommand cmd)
        {
            GameObject go = new GameObject("FlowKitController");
            go.AddComponent<FKEngine>();

            GameObjectUtility.SetParentAndAlign(go, cmd.context as GameObject);

            Undo.RegisterCreatedObjectUndo(go, "Create FlowKit Controller");

            Selection.activeObject = go;
            Selection.activeGameObject = go;
        }
    }
}
