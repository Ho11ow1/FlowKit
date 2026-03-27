using System;
using System.Collections;

using UnityEngine;

using FlowKit.Events;

namespace FlowKit
{
    public sealed class FKHandle
    {
        public static readonly FKHandle Invalid = new FKHandle();

        private readonly MonoBehaviour _owner;
        private readonly Func<IEnumerator> _funcRef;
        private readonly FKEventData _eventData;
        private readonly Action _onStop;

        private Coroutine routine;
        private Coroutine innerRoutine;
        private uint repeats = 0;
        private uint delaySeconds = 0;

        public bool IsAnimating { get; private set; } = false;
        public bool IsValid { get; private set; } = false;

        public FKHandle() {}

        public FKHandle(MonoBehaviour owner, Func<IEnumerator> funcRef, FKEventData eventData, Action onStop = null)
        {
            _owner = owner;
            _funcRef = funcRef;
            _eventData = eventData;
            _onStop = onStop;

            IsValid = true;
        }

        public FKHandle Play()
        {
            if (!IsValid)
            {
                return Invalid;
            }

            routine = _owner.StartCoroutine(PlayRoutine());
            IsAnimating = true;

            return this;
        }

        private IEnumerator PlayRoutine()
        {
            uint count = 0;
            do
            {
                if (count != 0 && delaySeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(delaySeconds);               
                }
                innerRoutine = _owner.StartCoroutine(_funcRef());
                yield return innerRoutine;

                count += 1;
            }
            while (count < repeats);

            IsAnimating = false;
            _onStop?.Invoke();
        }

        public FKHandle Repeat(uint count)
        {
            if (!IsAnimating)
            {
                repeats = count;
            }

            return this;
        }

        public FKHandle Delay(uint seconds)
        {
            if (!IsAnimating)
            {
                delaySeconds = seconds;
            }

            return this;
        }

        public IEnumerator Await()
        {
            while (IsAnimating)
            {
                yield return null;
            }
        }

        public void Stop()
        {
            if (!IsValid || routine == null)
            {
                return;
            }
            if (innerRoutine != null)
            {
                _owner.StopCoroutine(innerRoutine);
                innerRoutine = null;
            }

            _owner.StopCoroutine(routine);
            routine = null;
            
            IsAnimating = false;
            FlowKitEvents.InvokeEnd(_eventData);
            _onStop?.Invoke();
        }
    }
}
