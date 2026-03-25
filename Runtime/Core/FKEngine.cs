using UnityEngine;

namespace FlowKit
{
    [DefaultExecutionOrder(-10)]
    public partial class FKEngine : MonoBehaviour
    {
        public FKMovement Movement { get; private set; }
        public FKRotation Rotation { get; private set; }
        public FKScale Scale { get; private set;}
        public FKText Text { get; private set; }
        public FKVisibility Visibility { get; private set; }

        private void Awake()
        {
            Movement = gameObject.AddComponent<FKMovement>();
            Rotation = gameObject.AddComponent<FKRotation>();
            Scale = gameObject.AddComponent<FKScale>();
            Text = gameObject.AddComponent<FKText>();
            Visibility = gameObject.AddComponent<FKVisibility>();
        }
    }
}
