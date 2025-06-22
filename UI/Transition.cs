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
* FlowKit - Transition Animation Component
* Created by Hollow1
* 
* Applies a position transition animation to a UI component
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using FlowKit.Common;

namespace FlowKit.UI
{
    [AddComponentMenu("")]
    internal class Transition
    {
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;
        private readonly RectTransform _panelTransform;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly List<Utils.AutoIncreaseList<Vector2>> originalPosition = new List<Utils.AutoIncreaseList<Vector2>>()
        {
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> storedPosition = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public Transition(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void TransitionFromUp(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, -offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromDown(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {    
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, -offset, duration, delay, easing));
                    break;
            }
        }

        public void TransitionToUp(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToDown(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, offset, duration, delay, easing));
                    break;
            }
        }

        // ----------------------------------------------------- FROM TRANSITION -----------------------------------------------------

        private IEnumerator TransitionFrom(RectTransform component, Vector2 offset, float duration, float delay, EasingType easing)
        {
            if (component == null) { yield break; }

            Vector2 startPos, targetPos;

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            targetPos = component.anchoredPosition;
            startPos = targetPos - offset;
            component.anchoredPosition = startPos;

            float elapsedTime = 0f;
            FlowKitEvents.InvokeTransitionStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);

                component.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            component.anchoredPosition = targetPos;
            FlowKitEvents.InvokeTransitionEnd();
        }

        // ----------------------------------------------------- TO TRANSITION -----------------------------------------------------

        private IEnumerator TransitionTo(RectTransform component, int occurrence, Vector2 offset, float duration, float delay, EasingType easing)
        {
            if (component == null) { yield break; }

            Vector2 startPos = Vector2.zero;
            Vector2 targetPos;

            if (component == _panelTransform)
            {
                if (!storedPosition[FlowKitConstants.PanelIndex][0])
                {
                    startPos = _panelTransform.anchoredPosition;
                    originalPosition[FlowKitConstants.PanelIndex][0] = startPos;
                    storedPosition[FlowKitConstants.PanelIndex][0] = true;
                }
                else
                {
                    startPos = originalPosition[FlowKitConstants.PanelIndex][0];
                }
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedPosition[FlowKitConstants.TextIndex][occurrence]) 
                {
                    startPos = _textComponent[occurrence].rectTransform.anchoredPosition;
                    originalPosition[FlowKitConstants.TextIndex][occurrence] = startPos;
                    storedPosition[FlowKitConstants.TextIndex][occurrence] = true; 
                }
                else 
                {
                    startPos = originalPosition[FlowKitConstants.TextIndex][occurrence];
                }
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedPosition[FlowKitConstants.ImageIndex][occurrence]) 
                {
                    startPos = _imageComponent[occurrence].rectTransform.anchoredPosition;
                    originalPosition[FlowKitConstants.ImageIndex][occurrence] = startPos;
                    storedPosition[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                else 
                {
                    startPos = originalPosition[FlowKitConstants.ImageIndex][occurrence];
                }
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                {
                    startPos = ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition;
                    originalPosition[FlowKitConstants.ButtonIndex][occurrence] = startPos;
                    storedPosition[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                else
                {
                    startPos = originalPosition[FlowKitConstants.ButtonIndex][occurrence];
                }
            }

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            targetPos = startPos + offset;

            float elapsedTime = 0f;
            FlowKitEvents.InvokeTransitionStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);

                component.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            component.anchoredPosition = targetPos;
            FlowKitEvents.InvokeTransitionEnd();
        }
    }
}