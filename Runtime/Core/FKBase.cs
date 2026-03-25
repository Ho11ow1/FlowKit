using UnityEngine;

namespace FlowKit
{
    public abstract class FKBase : MonoBehaviour
    {
        [SerializeField] protected EasingType defaultEasing = EasingType.Linear;

        public RectTransform RectTransform { get; protected set; }
        public bool IsAnimating { get; protected set; }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void Stop()
        {
            StopAllCoroutines();
            IsAnimating = false;
        }
    }
}
