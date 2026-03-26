using System;
using System.Collections;

using UnityEngine;

namespace FlowKit
{
    public sealed class FKHandle
    {
        public static readonly FKHandle Invalid = new FKHandle();

        private readonly MonoBehaviour _owner;
        private readonly Func<IEnumerator> _funcRef;
        private readonly Action _onStop;
        private Coroutine routine;

        public bool IsAnimating { get; private set; } = false;
        public bool IsValid { get; private set; } = false;

        public FKHandle() {}

        public FKHandle(MonoBehaviour owner, Func<IEnumerator> funcRef, Action onStop = null)
        {
            _owner = owner;
            _funcRef = funcRef;
            _onStop = onStop;

            IsValid = true;
        }

        public FKHandle Play()
        {
            if (!IsValid)
            {
                return Invalid;
            }

            routine = _owner.StartCoroutine(_funcRef());
            IsAnimating = true;

            return this;
        }

        public void Stop()
        {
            if (!IsValid || routine == null)
            {
                return;
            }

            _owner.StopCoroutine(routine);
            routine = null;
            
            IsAnimating = false;
            _onStop?.Invoke();
        }
    }
}
