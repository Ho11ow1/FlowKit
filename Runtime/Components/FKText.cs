using System.Collections;

using UnityEngine;

using TMPro;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKText : FKBase
    {
        #region FireAndForget
        // =============== Component Self =============== \\
        /// <summary>
        /// Applies a smooth wave effect to this component's <see cref="TMP_Text"/>.
        /// <para>If no duration is provided, the wave runs indefinitely until stopped externally.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="TMP_Text"/> is found on this component.</para>
        /// </summary>
        /// <param name="amplitude">Height of the wave in local units.</param>
        /// <param name="frequency">Speed of the wave cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        public void Wave(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => Wave(RectTransform, amplitude, frequency, duration, delay);
        /// <summary>
        /// Applies a horizontal shake effect to this component's <see cref="TMP_Text"/>, displacing each character along the X axis using a sine wave.
        /// <para>If no duration is provided, the shake runs indefinitely until stopped externally.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <see cref="RectTransform"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="TMP_Text"/> is found on this component.</para>
        /// </summary>
        /// <param name="amplitude">Width of the shake in local units.</param>
        /// <param name="frequency">Speed of the shake cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        public void Shake(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => Shake(RectTransform, amplitude, frequency, duration, delay);

        // =============== Monolith via Reference =============== \\
        /// <summary>
        /// Applies a smooth wave effect to the <see cref="TMP_Text"/> on the specified RectTransform.
        /// <para>If no duration is provided, the wave runs indefinitely until stopped externally.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="TMP_Text"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="TMP_Text"/> will be animated.</param>
        /// <param name="amplitude">Height of the wave in local units.</param>
        /// <param name="frequency">Speed of the wave cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        public void Wave(RectTransform obj, float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKText>(nameof(Wave), gameObject.name);
                return;
            }
            if (!obj.TryGetComponent<TMP_Text>(out var txt))
            {
                FKLogger.MissingComponent<FKText>(typeof(TMP_Text), obj.name);
                return;
            }

            if (duration.HasValue)
            {
                StartCoroutine(WaveImpl(txt, amplitude, frequency, duration.Value, delay, GenerateEventData(obj, duration.Value)));
            }
            else
            {
                StartCoroutine(InfiniteWaveImpl(txt, amplitude, frequency, delay, GenerateEventData(obj, float.PositiveInfinity)));
            }
        }
        /// <summary>
        /// Applies a horizontal shake effect to the <see cref="TMP_Text"/> on the specified RectTransform, displacing each character along the X axis using a sine wave.
        /// <para>If no duration is provided, the shake runs indefinitely until stopped externally.</para>
        /// <para>Logs a null warning via <see cref="FKLogger"/> and returns early if <paramref name="obj"/> is null.</para>
        /// <para>Logs a missing component warning and returns early if no <see cref="TMP_Text"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="TMP_Text"/> will be animated.</param>
        /// <param name="amplitude">Width of the shake in local units.</param>
        /// <param name="frequency">Speed of the shake cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        public void Shake(RectTransform obj, float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKText>(nameof(Shake), gameObject.name);
                return;
            }
            if (!obj.TryGetComponent<TMP_Text>(out var txt))
            {
                FKLogger.MissingComponent<FKText>(typeof(TMP_Text), obj.name);
                return;
            }

            if (duration.HasValue)
            {
                StartCoroutine(ShakeImpl(txt, amplitude, frequency, duration.Value, delay, GenerateEventData(obj, duration.Value)));
            }
            else
            {
                StartCoroutine(InfiniteShakeImpl(txt, amplitude, frequency, delay, GenerateEventData(obj, float.PositiveInfinity)));
            }
        }
        #endregion
        #region Handles
        // =============== Component Self =============== \\
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a wave effect on this component's <see cref="TMP_Text"/>.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>If no duration is provided, the wave runs indefinitely until stopped externally.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null or no <see cref="TMP_Text"/> is found on this component.</para>
        /// </summary>
        /// <param name="amplitude">Height of the wave in local units.</param>
        /// <param name="frequency">Speed of the wave cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle WaveHandle(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => WaveHandle(RectTransform, amplitude, frequency, duration, delay);
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a horizontal shake effect on this component's <see cref="TMP_Text"/>.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>If no duration is provided, the shake runs indefinitely until stopped externally.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <see cref="RectTransform"/> is null or no <see cref="TMP_Text"/> is found on this component.</para>
        /// </summary>
        /// <param name="amplitude">Width of the shake in local units.</param>
        /// <param name="frequency">Speed of the shake cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation.</returns>
        public FKHandle ShakeHandle(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => ShakeHandle(RectTransform, amplitude, frequency, duration, delay);

        // =============== Monolith via Reference =============== \\

        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a wave effect on the <see cref="TMP_Text"/> of the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>If no duration is provided, the wave runs indefinitely until stopped externally. When stopped, a mesh update is forced to clear any residual vertex offsets.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null or no <see cref="TMP_Text"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="TMP_Text"/> will be animated.</param>
        /// <param name="amplitude">Height of the wave in local units.</param>
        /// <param name="frequency">Speed of the wave cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle WaveHandle(RectTransform obj, float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKText>(nameof(WaveHandle), gameObject.name);
                return FKHandle.Invalid;
            }
            if (!obj.TryGetComponent<TMP_Text>(out var txt))
            {
                FKLogger.MissingComponent<FKText>(typeof(TMP_Text), obj.name);
                return FKHandle.Invalid;
            }

            if (duration.HasValue)
            {
                var eventData = GenerateEventData(obj, duration.Value);
                return new FKHandle(this,
                    () => WaveImpl(txt, amplitude, frequency, duration.Value, delay, eventData),
                    eventData);
            }
            else
            {
                var eventData = GenerateEventData(obj, float.PositiveInfinity);
                return new FKHandle(this,
                    () => InfiniteWaveImpl(txt, amplitude, frequency, delay, eventData),
                    eventData,
                    () => eventData.Target.GetComponent<TMP_Text>().ForceMeshUpdate());
            }
        }
        /// <summary>
        /// Returns an <see cref="FKHandle"/> for a horizontal shake effect on the <see cref="TMP_Text"/> of the specified RectTransform.
        /// <para>The animation does not start until the handle is played.</para>
        /// <para>If no duration is provided, the shake runs indefinitely until stopped externally. When stopped, a mesh update is forced to clear any residual vertex offsets.</para>
        /// <para>Returns <see cref="FKHandle.Invalid"/> if <paramref name="obj"/> is null or no <see cref="TMP_Text"/> is found on <paramref name="obj"/>.</para>
        /// </summary>
        /// <param name="obj">RectTransform whose <see cref="TMP_Text"/> will be animated.</param>
        /// <param name="amplitude">Width of the shake in local units.</param>
        /// <param name="frequency">Speed of the shake cycle.</param>
        /// <param name="duration">Length of the effect in seconds. If null, runs indefinitely.</param>
        /// <param name="delay">Delay in seconds before the effect starts.</param>
        /// <returns>An <see cref="FKHandle"/> bound to the animation, or <see cref="FKHandle.Invalid"/> on failure.</returns>
        public FKHandle ShakeHandle(RectTransform obj, float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
        {
            if (obj == null)
            {
                FKLogger.NullObject<FKText>(nameof(ShakeHandle), gameObject.name);
                return FKHandle.Invalid;
            }
            if (!obj.TryGetComponent<TMP_Text>(out var txt))
            {
                FKLogger.MissingComponent<FKText>(typeof(TMP_Text), obj.name);
                return FKHandle.Invalid;
            }

            if (duration.HasValue)
            {
                var eventData = GenerateEventData(obj, duration.Value);
                return new FKHandle(this,
                    () => ShakeImpl(txt, amplitude, frequency, duration.Value, delay, eventData),
                    eventData);
            }
            else
            {
                var eventData = GenerateEventData(obj, float.PositiveInfinity);
                return new FKHandle(this,
                    () => InfiniteShakeImpl(txt, amplitude, frequency, delay, eventData),
                    eventData,
                    () => eventData.Target.GetComponent<TMP_Text>().ForceMeshUpdate());
            }
        }
        #endregion
        // ============================== ACTUAL LOGIC ============================== \\

        private IEnumerator WaveImpl(TMP_Text tmp, float amplitude, float frequency, float duration, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMPVertexUtils.ApplySineWave(Vertecies.Y, textInfo, elapsedTime, frequency, i, amplitude);
                }
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            tmp.ForceMeshUpdate();

            FlowKitEvents.InvokeEnd(data);
        }

        private IEnumerator InfiniteWaveImpl(TMP_Text tmp, float amplitude, float frequency, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            float elapsedTime = 0f;
            while (true)
            {
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMPVertexUtils.ApplySineWave(Vertecies.Y, textInfo, elapsedTime, frequency, i, amplitude);
                }
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
        }

        private IEnumerator ShakeImpl(TMP_Text tmp, float amplitude, float frequency, float duration, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMPVertexUtils.ApplySineWave(Vertecies.X, textInfo, elapsedTime, frequency, i, amplitude);
                }
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            tmp.ForceMeshUpdate();

            FlowKitEvents.InvokeEnd(data);
        }

        private IEnumerator InfiniteShakeImpl(TMP_Text tmp, float amplitude, float frequency, float delay, FKEventData data)
        {
            if (delay > 0f)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            FlowKitEvents.InvokeStart(data);

            tmp.ForceMeshUpdate();
            var textInfo = tmp.textInfo;

            float elapsedTime = 0f;
            while (true)
            {
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMPVertexUtils.ApplySineWave(Vertecies.X, textInfo, elapsedTime, frequency, i, amplitude);
                }
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
        }
    }
}
