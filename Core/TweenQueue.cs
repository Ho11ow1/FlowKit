using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit.Core
{
    public class TweenQueue : MonoBehaviour
    {
        public void Queue(GameObject gameObject, AnimationStep[] steps, string name, bool autoStart = false)
        {

            if (autoStart) { StartQueue(); }
        }

        public void StartQueue()
        {

        }

        private IEnumerator RunQueue()
        {
            yield return null;
        }
    }

    public class AnimationStep
    {
        public UnityAction action { get; }
        public float delay { get; }

        public AnimationStep(UnityAction action, float delay = 0f)
        {
            this.action = action;
            this.delay = delay;
        }

        public static AnimationStep Call(UnityAction action) => new AnimationStep(action);
        public static AnimationStep Wait(float duration) => new AnimationStep(() => {}, duration);
    }
}
