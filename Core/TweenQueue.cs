using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit.Core
{
    public class TweenQueue : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<AnimationStep>> _namedQueues = new Dictionary<string, Queue<AnimationStep>>();
        private readonly Dictionary<string, Coroutine> _runningQueues = new Dictionary<string, Coroutine>();
        private readonly Dictionary<string, bool> _pausedQueues = new Dictionary<string, bool>();

        [System.Obsolete("This is an experimental feature and may not work just yet")] 
        public void Queue(GameObject gameObject, AnimationStep[] steps, string name, bool autoStart = false)
        {
            Queue<AnimationStep> queue = new Queue<AnimationStep>();
            foreach (var step in steps)
            {
                queue.Enqueue(step);
            }
            _namedQueues[name] = queue;

            if (autoStart) { StartQueue(name); }
        }

        [System.Obsolete("This is an experimental feature and may not work just yet")] 
        public void StartQueue(string name)
        {
            if (!_runningQueues.ContainsKey(name))
            {
                var coroutine = StartCoroutine(RunQueue(name));
                _runningQueues[name] = coroutine;
                _pausedQueues[name] = false;
            }
        }

        [System.Obsolete("This is an experimental feature and may not work just yet")] 
        public void StopQueue(string name)
        {
            if (_runningQueues.TryGetValue(name, out var coroutine))
            {
                StopCoroutine(coroutine);
                _runningQueues.Remove(name);
                _pausedQueues.Remove(name);
                _namedQueues.Remove(name);
            }
        }

        [System.Obsolete("This is an experimental feature and may not work just yet")] 
        public void PauseQueue(string name)
        {
            if (_runningQueues.ContainsKey(name)) { _pausedQueues[name] = true; }
        }

        [System.Obsolete("This is an experimental feature and may not work just yet")] 
        public void ResumeQueue(string name)
        {
            if (_runningQueues.ContainsKey(name)) { _pausedQueues[name] = false; }
        }

        private IEnumerator RunQueue(string name)
        {
            if (!_namedQueues.TryGetValue(name, out var queue)) { yield break; }
            while (queue.Count > 0)
            {
                yield return new WaitUntil(() => !_pausedQueues.GetValueOrDefault(name, false));

                var step = queue.Dequeue();

                if (step.delay > 0f) { yield return new WaitForSeconds(step.delay); }

                step.action?.Invoke();
            }

            _runningQueues.Remove(name);
            _pausedQueues.Remove(name);
            _namedQueues.Remove(name);
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
