using System.Collections;

using UnityEngine;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKRotation : FKBase
    {
        #region FireAndForget
        // =============== Component Self =============== \\
        /// <summary>
        /// Rotates this component's RectTransform by the specified degrees over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="degrees">Total degrees to rotate. Positive values rotate clockwise.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the rotation.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Rotate(float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => Rotate(RectTransform, degrees, duration, easing, delay);
        /// <summary>
        /// Spins this component's RectTransform continuously at a constant rate.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="degreesPerSecond">Rotation speed in degrees per second. Positive values rotate clockwise.</param>
        /// <param name="delay">Delay in seconds before the spin starts.</param>
        public void Spin(float degreesPerSecond, float delay = 0f)
            => Spin(RectTransform, degreesPerSecond, delay);
        /// <summary>
        /// Instantly sets this component's RectTransform rotation to the specified angle.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="degrees">Target rotation in degrees. Positive values rotate clockwise.</param>
        public void SetRotation(float degrees)
            => SetRotation(RectTransform, degrees);
        /// <summary>
        /// Instantly sets this component's RectTransform rotation using a full rotation vector.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="rotationVector">Target rotation as Euler angles. Positive z coordinate sets clockwise rotation</param>
        public void SetRotation(Vector3 rotationVector)
            => SetRotation(RectTransform, rotationVector);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Rotates the specified RectTransform by the specified degrees over a given duration.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="degrees">Total degrees to rotate. Positive values rotate clockwise.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the rotation.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        public void Rotate(RectTransform obj, float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(Rotate), gameObject.name);
                return;
            }

            StartCoroutine(RotateImpl(obj, degrees, duration, easing, delay, GenerateEventData(obj, duration)));
        }
        /// <summary>
        /// Spins the specified RectTransform continuously at a constant rate.
        /// <para>Runs indefinitely until stopped externally. Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="degreesPerSecond">Rotation speed in degrees per second. Positive values rotate clockwise.</param>
        /// <param name="delay">Delay in seconds before the spin starts.</param>
        public void Spin(RectTransform obj, float degreesPerSecond, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(Spin), gameObject.name);
                return;
            }

            StartCoroutine(SpinImpl(obj, degreesPerSecond, delay, GenerateEventData(obj, float.PositiveInfinity)));
        }
        /// <summary>
        /// Instantly sets the rotation of the specified RectTransform to the specified angle.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to reposition.</param>
        /// <param name="degrees">Target rotation in degrees. Positive values rotate clockwise.</param>
        public void SetRotation(RectTransform obj, float degrees)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(SetRotation), gameObject.name);
                return;
            }

            obj.localRotation = Quaternion.Euler(0, 0, -degrees);
        }
        /// <summary>
        /// Instantly sets the rotation of the specified RectTransform using a full rotation vector.
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to reposition.</param>
        /// <param name="rotationVector">Target rotation as Euler angles. Positive z coordinate sets clockwise rotation</param>
        public void SetRotation(RectTransform obj, Vector3 rotationVector)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKRotation>(nameof(SetRotation), gameObject.name);
                return;
            }

            obj.localRotation = Quaternion.Euler(rotationVector.x, rotationVector.y, -rotationVector.z);
        }
        #endregion
        #region Handles
        // =============== Component Self =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a rotation animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="degrees">Total degrees to rotate. Positive values rotate clockwise.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the rotation.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle RotateHandle(float degrees, float duration, EasingType easing = EasingType.Linear, float delay = 0f)
            => RotateHandle(RectTransform, degrees, duration, easing, delay);
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a continuous spin animation on this component's RectTransform.
        /// <para>The animation does not start until the handle is played. Runs indefinitely until stopped externally.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null.</para>
        /// </summary>
        /// <param name="degreesPerSecond">Rotation speed in degrees per second. Positive values rotate clockwise.</param>
        /// <param name="delay">Delay in seconds before the spin starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle SpinHandle(float degreesPerSecond, float delay = 0f)
            => SpinHandle(RectTransform, degreesPerSecond, delay);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a rotation animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="degrees">Total degrees to rotate. Positive values rotate clockwise.</param>
        /// <param name="duration">Length of the animation in seconds.</param>
        /// <param name="easing">Easing function applied to the rotation.</param>
        /// <param name="delay">Delay in seconds before the animation starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
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
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a continuous spin animation on the specified RectTransform.
        /// <para>The animation does not start until the handle is played. Runs indefinitely until stopped externally.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null.</para>
        /// </summary>
        /// <param name="obj">RectTransform to animate.</param>
        /// <param name="degreesPerSecond">Rotation speed in degrees per second. Positive values rotate clockwise.</param>
        /// <param name="delay">Delay in seconds before the spin starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
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
        #endregion
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
    }
}
