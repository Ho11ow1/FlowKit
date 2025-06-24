/* Copyright 2025 Hollow1
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* --------------------------------------------------------
* FlowKit - Queue API component
* Created by Hollow1
* 
* Provides a queue-system best used for
* single target complex animations
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit.Core
{
    internal class FlowKitQueue
    {
        private readonly Dictionary<string, Queue<AnimationStep>> _namedQueues = new Dictionary<string, Queue<AnimationStep>>();
        private readonly Dictionary<string, Coroutine> _runningQueues = new Dictionary<string, Coroutine>();
        private readonly Dictionary<string, bool> _pausedQueues = new Dictionary<string, bool>();

        private readonly MonoBehaviour _monoBehaviour;

        public FlowKitQueue(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public void Queue(AnimationStep[] steps, string name, bool autoStart = false)
        {
            Queue<AnimationStep> queue = new Queue<AnimationStep>();
            foreach (var step in steps)
            {
                queue.Enqueue(step);
            }
            _namedQueues[name] = queue;

            if (autoStart) { Start(name); }
        }

        public void Start(string name)
        {
            if (_namedQueues.ContainsKey(name) && !_runningQueues.ContainsKey(name))
            {
                var coroutine = _monoBehaviour.StartCoroutine(RunQueue(name));
                _runningQueues[name] = coroutine;
            }
        }

        public void Stop(string name)
        {
            if (_namedQueues.ContainsKey(name) && _runningQueues.ContainsKey(name))
            {
                _runningQueues.TryGetValue(name, out Coroutine coroutine);
                _monoBehaviour.StopCoroutine(coroutine);

                _namedQueues.Remove(name);
                _runningQueues.Remove(name);
                _pausedQueues.Remove(name);
            }
        }

        public void Pause(string name)
        {
            if (_namedQueues.ContainsKey(name) && _runningQueues.ContainsKey(name))
            {
                _pausedQueues[name] = true;
            }
        }

        public void Resume(string name)
        {
            if (_namedQueues.ContainsKey(name) && _runningQueues.ContainsKey(name))
            {
                _pausedQueues[name] = false;
            }
        }

        private IEnumerator RunQueue(string name)
        {
            var queue = _namedQueues[name];

            while (queue.Count > 0)
            {
                yield return _monoBehaviour.StartCoroutine(WaitPaused(name)); // Pause check 
                if (!_namedQueues.ContainsKey(name)) { break; }

                var step = queue.Dequeue();
                if (step.delay > 0f) 
                { 
                    yield return new WaitForSeconds(step.delay); 
                }

                step.action?.Invoke();
            }

            _runningQueues.Remove(name);
            _pausedQueues.Remove(name);
            _namedQueues.Remove(name);
        }

        private IEnumerator WaitPaused(string name)
        {
            while (_pausedQueues.ContainsKey(name) && _pausedQueues[name])
            {
                yield return null;
            }
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

        /// <summary>
        /// Creates a new AnimationStep that calls the specified method.
        /// </summary>
        /// <param name="action">Method to be called through a delegate lambda | () =></param>
        public static AnimationStep Call(UnityAction action) => new AnimationStep(action);

        /// <summary>
        /// Creates a new AnimationStep that waits for a specified duration.
        /// </summary>
        /// <param name="duration">Time in seconds to wait</param>
        public static AnimationStep Wait(float duration) => new AnimationStep(null, duration);
    }
}
