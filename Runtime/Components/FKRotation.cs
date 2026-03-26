using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public class FKRotation : FKBase
    {
        // ============================== VOIDS ============================== \\

        // =============== Component Self =============== \\
        public void Rotate(float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            StartCoroutine(RotateImpl(RectTransform, degrees, duration, easing, delay, GenerateEventData(RectTransform, duration)));
        }
        public void Spin(float degreesPerSecond, float delay = 0f)
        {
            StartCoroutine(SpinImpl(RectTransform, degreesPerSecond, delay, GenerateEventData(RectTransform, float.PositiveInfinity)));
        }

        // =============== Monolith via Reference ===============\\
        public void Rotate(RectTransform obj, float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            StartCoroutine(RotateImpl(obj, degrees, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void Spin(RectTransform obj, float degreesPerSecond, float delay = 0f)
        {
            StartCoroutine(SpinImpl(obj, degreesPerSecond, delay, GenerateEventData(obj, float.PositiveInfinity)));
        }

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public IEnumerator RotateRoutine(float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            return RotateImpl(RectTransform, degrees, duration, easing, delay, GenerateEventData(RectTransform, duration));
        }
        public IEnumerator SpinRoutine(float degreesPerSecond, float delay = 0f)
        {
            return SpinImpl(RectTransform, degreesPerSecond, delay, GenerateEventData(RectTransform, float.PositiveInfinity));
        }

        // =============== Monolith via Reference =============== \\
        public IEnumerator RotateRoutine(RectTransform obj, float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            return RotateImpl(obj, degrees, duration, easing, delay, GenerateEventData(obj, duration));
        }
        public IEnumerator SpinRoutine(RectTransform obj, float degreesPerSecond, float delay = 0f)
        {
            return SpinImpl(obj, degreesPerSecond, delay, GenerateEventData(obj, float.PositiveInfinity));
        }

        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator RotateImpl(RectTransform obj, float degrees, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            Events.FlowKitEvents.InvokeStart(data);

            var startRotation = obj.localRotation;
            var targetRotation = Quaternion.Euler(0, 0, obj.eulerAngles.z - degrees);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                var t = EasingUtils.Evaluate(easing, elapsedTime / duration);

                obj.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            obj.localRotation = targetRotation;

            Events.FlowKitEvents.InvokeEnd(data);
        }

        private IEnumerator SpinImpl(RectTransform obj, float degreesPerSecond, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            Events.FlowKitEvents.InvokeStart(data);

            while (true)
            {
                obj.localRotation *= Quaternion.Euler(0, 0, -degreesPerSecond * Time.unscaledDeltaTime);

                yield return null;
            }
            //
            // Will be able to be called in future with async cancellations / .Stop() jump to end label
            //
            Events.FlowKitEvents.InvokeEnd(data);
        }

        private FKEventData GenerateEventData(RectTransform target, float duration)
        {
            return new FKEventData(gameObject, AnimationType.Rotation, target, duration);
        }
    }
}
