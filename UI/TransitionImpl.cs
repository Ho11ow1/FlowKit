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
* Version: 1.2.0
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
    internal class TransitionImpl
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

        public TransitionImpl(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetPosition(AnimationTarget target, int occurrence, Vector2 newPosition)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(_panelTransform.gameObject, 0);

                    _panelTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(_textComponent[occurrence].gameObject, occurrence);

                    _textComponent[occurrence].rectTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(_imageComponent[occurrence].gameObject, occurrence);

                    _imageComponent[occurrence].rectTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(_buttonComponent[occurrence].gameObject, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition = newPosition;
                    break;
            }
        }

        public void TransitionFromTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, -offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(-offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionFromPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, -offset, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, -offset, duration, delay, easing));
                    break;
            }
        }

        public void TransitionToTop(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(0, offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToBottom(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(0, -offset), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToLeft(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(-offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToRight(AnimationTarget target, int occurrence, float offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, new Vector2(offset, 0), duration, delay, easing));
                    break;
            }
        }

        public void TransitionToPosition(AnimationTarget target, int occurrence, Vector2 offset, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, occurrence, offset, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, occurrence, offset, duration, delay, easing));
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    #if UNITY_EDITOR
                    if (!storedPosition[FlowKitConstants.PanelIndex][0])
                    {
                        Debug.LogError($"No saved scale found for Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _panelTransform.anchoredPosition = originalPosition[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    #if UNITY_EDITOR
                    if (!storedPosition[FlowKitConstants.TextIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Text component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _textComponent[occurrence].rectTransform.anchoredPosition = originalPosition[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    #if UNITY_EDITOR
                    if (!storedPosition[FlowKitConstants.ImageIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Image component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _imageComponent[occurrence].rectTransform.anchoredPosition = originalPosition[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    #if UNITY_EDITOR
                    if (!storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Button component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition = originalPosition[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        // ----------------------------------------------------- FROM TRANSITION -----------------------------------------------------

        private IEnumerator TransitionFrom(RectTransform component, Vector2 offset, float duration, float delay, EasingType easing)
        {
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
            GetStartPos(component, occurrence, out Vector2 startPos);
            Vector2 targetPos;

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

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private bool IndexNullChecksPass(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    return _panelTransform != null;
                case AnimationTarget.Text:
                    return occurrence < _textComponent.Length && _textComponent[occurrence] != null;
                case AnimationTarget.Image:
                    return occurrence < _imageComponent.Length && _imageComponent[occurrence] != null;
                case AnimationTarget.Button:
                    return occurrence < _buttonComponent.Length && _buttonComponent[occurrence] != null;
                default:
                    return false;
            }
        }

        private void SavePosition(GameObject component, int occurrence)
        {
            if (component == _panelTransform.gameObject)
            {
                if (!storedPosition[FlowKitConstants.PanelIndex][0])
                {
                    originalPosition[FlowKitConstants.PanelIndex][0] = _panelTransform.anchoredPosition;
                    storedPosition[FlowKitConstants.PanelIndex][0] = true;
                }
                return;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedPosition[FlowKitConstants.TextIndex][occurrence])
                {
                    originalPosition[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.anchoredPosition;
                    storedPosition[FlowKitConstants.TextIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                if (!storedPosition[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalPosition[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.anchoredPosition;
                    storedPosition[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                if (!storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalPosition[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition;
                    storedPosition[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                return;
            }
        }

        private void GetStartPos(RectTransform component, int occurrence, out Vector2 startPos)
        {
            startPos = Vector2.zero;

            if (component == _panelTransform)
            {
                if (!storedPosition[FlowKitConstants.PanelIndex][0])
                {
                    SavePosition(_panelTransform.gameObject, occurrence);
                }
                startPos = _panelTransform.anchoredPosition;
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedPosition[FlowKitConstants.TextIndex][occurrence])
                {
                    SavePosition(_textComponent[occurrence].gameObject, occurrence);
                }
                startPos = _textComponent[occurrence].rectTransform.anchoredPosition;
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedPosition[FlowKitConstants.ImageIndex][occurrence])
                {
                    SavePosition(_imageComponent[occurrence].gameObject, occurrence);
                }
                startPos = _imageComponent[occurrence].rectTransform.anchoredPosition;
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                {
                    SavePosition(_buttonComponent[occurrence].gameObject, occurrence);
                }
                startPos = ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition;
            }
        }
    }
}
