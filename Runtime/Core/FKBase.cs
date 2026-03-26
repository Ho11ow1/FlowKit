using UnityEngine;

namespace FlowKit
{
    public abstract class FKBase : MonoBehaviour
    {
        public RectTransform RectTransform { get; protected set; }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}
