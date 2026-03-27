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
            => Rotate(RectTransform, degrees, duration, easing, delay);
        public void Spin(float degreesPerSecond, float delay = 0f)
            => Spin(RectTransform, degreesPerSecond, delay);
        public void SetRotation(float degrees)
            => SetRotation(RectTransform, degrees);
        public void SetRotation(Vector3 rotationVector)
            => SetRotation(RectTransform, rotationVector);

        // =============== Monolith via Reference ===============\\
        public void Rotate(RectTransform obj, float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(Rotate), gameObject.name);
                return;
            }

            StartCoroutine(RotateImpl(obj, degrees, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        public void Spin(RectTransform obj, float degreesPerSecond, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(Spin), gameObject.name);
                return;
            }

            StartCoroutine(SpinImpl(obj, degreesPerSecond, delay, GenerateEventData(obj, float.PositiveInfinity)));
        }
        public void SetRotation(RectTransform obj, float degrees)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(SetRotation), gameObject.name);
                return;
            }

            obj.localRotation = Quaternion.Euler(0, 0, -degrees);
        }
        public void SetRotation(RectTransform obj, Vector3 rotationVector)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(SetRotation), gameObject.name);
                return;
            }

            obj.localRotation = Quaternion.Euler(rotationVector.x, rotationVector.y, -rotationVector.z);
        }

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public FKHandle RotateHandle(float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => RotateHandle(RectTransform, degrees, duration, easing, delay);
        public FKHandle SpinHandle(float degreesPerSecond, float delay = 0f)
            => SpinHandle(RectTransform, degreesPerSecond, delay);

        // =============== Monolith via Reference =============== \\
        public FKHandle RotateHandle(RectTransform obj, float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(RotateHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, duration);
            return new FKHandle(this,
                () => RotateImpl(obj, degrees, duration, easing, delay, eventData),
                eventData);
        }
        public FKHandle SpinHandle(RectTransform obj, float degreesPerSecond, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(SpinHandle), gameObject.name);
                return FKHandle.Invalid;
            }

            var eventData = GenerateEventData(obj, float.PositiveInfinity);
            return new FKHandle(this,
                () => SpinImpl(obj, degreesPerSecond, delay, eventData),
                eventData);
        }

        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator RotateImpl(RectTransform obj, float degrees, float duration, EasingType easing, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

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

            FlowKitEvents.InvokeEnd(data);
        }

        private IEnumerator SpinImpl(RectTransform obj, float degreesPerSecond, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            while (true)
            {
                obj.localRotation *= Quaternion.Euler(0, 0, -degreesPerSecond * Time.unscaledDeltaTime);

                yield return null;
            }
        }

        private FKEventData GenerateEventData(RectTransform target, float duration)
        {
            return new FKEventData(gameObject, AnimationType.Rotation, target, duration);
        }
    }
}
