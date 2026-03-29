using UnityEngine;

using FlowKit.Events;

namespace FlowKit
{
    public abstract class FKBase : MonoBehaviour
    {
        public RectTransform RectTransform { get; protected set; }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        protected FKEventData GenerateEventData(RectTransform target, float duration)
            => new FKEventData(gameObject, ResolveAnimationType(), target, duration);

        private AnimationType ResolveAnimationType()
        {
            return this switch
            {
                FKMovement =>  AnimationType.Movement,
                FKRotation =>  AnimationType.Rotation,
                FKScale =>  AnimationType.Scale,
                FKVisibility =>  AnimationType.Visibility,
                FKText =>  AnimationType.Text,
                _ => AnimationType.Unknown
            };
        }
    }
}
