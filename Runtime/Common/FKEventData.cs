using UnityEngine;

namespace FlowKit.Events
{
    public readonly struct FKEventData
    {
        public readonly GameObject Source;
        public readonly AnimationType AnimationType;
        public readonly RectTransform Target;
        public readonly float Duration;

        public FKEventData(GameObject source, AnimationType animationType, RectTransform target, float duration)
        {
            Source = source;
            AnimationType = animationType;
            Target = target;
            Duration = duration;
        }

        /// <returns>A formatted string of all the data inside of this struct</returns>
        public override string ToString()
        {
            return $"[FlowKit] {AnimationType} on '{Target.name}' from '{Source.name}' ({(float.IsPositiveInfinity(Duration) ? "Infinity" : $"{Duration}s")})";
        }
    }
}
