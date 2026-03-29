using UnityEngine;

namespace FlowKit
{
    [RequireComponent(typeof(FKMovement))]
    [RequireComponent(typeof(FKRotation))]
    [RequireComponent(typeof(FKScale))]
    [RequireComponent(typeof(FKText))]
    [RequireComponent(typeof(FKVisibility))]
    public class FKEngine : MonoBehaviour
    {
        public FKMovement Movement { get; private set; }
        public FKRotation Rotation { get; private set; }
        public FKScale Scale { get; private set;}
        public FKText Text { get; private set; }
        public FKVisibility Visibility { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<FKMovement>();
            Rotation = GetComponent<FKRotation>();
            Scale = GetComponent<FKScale>();
            Text = GetComponent<FKText>();
            Visibility = GetComponent<FKVisibility>();
        }
    }
}
