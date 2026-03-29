using System.Collections;

using UnityEngine;

using TMPro;

using FlowKit.Events;
using FlowKit.Utils;

namespace FlowKit
{
    public sealed class FKText : FKBase
    {
        // ============================== VOIDS ============================== \\

        // =============== Component Self =============== \\
        public void Wave(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => Wave(RectTransform, amplitude, frequency, duration, delay);
        public void Shake(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => Shake(RectTransform, amplitude, frequency, duration, delay);

        // =============== Monolith via Reference =============== \\
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

        // ============================== ENUMERATORS ============================== \\

        // =============== Component Self =============== \\
        public FKHandle WaveHandle(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => WaveHandle(RectTransform, amplitude, frequency, duration, delay);
        public FKHandle ShakeHandle(float amplitude = 0.2f, float frequency = 4f, float? duration = null, float delay = 0f)
            => ShakeHandle(RectTransform, amplitude, frequency, duration, delay);

        // =============== Monolith via Reference =============== \\
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
