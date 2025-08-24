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
* Version: 1.3.0
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
    internal class MovementImpl
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly RectTransform _panelTransform;
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;

        private readonly List<Utils.AutoIncreaseList<Vector2>> _originalPosition = new List<Utils.AutoIncreaseList<Vector2>>()
        {
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> _storedPosition = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public MovementImpl(MonoBehaviour runner, RectTransform panel, TextMeshProUGUI[] text, Image[] image, Button[] button)
        {
            _monoBehaviour = runner;
            _panelTransform = panel;
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetPosition(AnimationTarget target, int occurrence, Vector2 newPosition)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _panelTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _textComponent[occurrence].rectTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _imageComponent[occurrence].rectTransform.anchoredPosition = newPosition;
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition = newPosition;
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedPosition[FlowKitConstants.PanelIndex][0])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _panelTransform.anchoredPosition = _originalPosition[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    if (!_storedPosition[FlowKitConstants.TextIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Text component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _textComponent[occurrence].rectTransform.anchoredPosition = _originalPosition[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    if (!_storedPosition[FlowKitConstants.ImageIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Image component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _imageComponent[occurrence].rectTransform.anchoredPosition = _originalPosition[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    if (!_storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition = _originalPosition[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        public void TransitionFromTop(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, -offset), duration, easing, delay));
                    break;
            }
        }

        public void TransitionFromBottom(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(0, offset), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(0, offset), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(0, offset), duration, easing, delay));
                    break;
            }
        }

        public void TransitionFromLeft(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(offset, 0), duration, easing, delay));
                    break;
            }
        }

        public void TransitionFromRight(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, new Vector2(-offset, 0), duration, easing, delay));
                    break;
            }
        }

        public void TransitionFromPosition(AnimationTarget target, int occurrence, Vector2 offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_panelTransform, -offset, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_textComponent[occurrence].rectTransform, -offset, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom(_imageComponent[occurrence].rectTransform, -offset, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(TransitionFrom((RectTransform)_buttonComponent[occurrence].transform, -offset, duration, easing, delay));
                    break;
            }
        }

        public void TransitionToTop(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, target, occurrence, new Vector2(0, offset), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, target, occurrence, new Vector2(0, offset), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, target, occurrence, new Vector2(0, offset), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, new Vector2(0, offset), duration, easing, delay));
                    break;
            }
        }

        public void TransitionToBottom(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, target, occurrence, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, target, occurrence, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, target, occurrence, new Vector2(0, -offset), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, new Vector2(0, -offset), duration, easing, delay));
                    break;
            }
        }

        public void TransitionToLeft(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, target, occurrence, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, target, occurrence, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, target, occurrence, new Vector2(-offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, new Vector2(-offset, 0), duration, easing, delay));
                    break;
            }
        }

        public void TransitionToRight(AnimationTarget target, int occurrence, float offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, target, occurrence, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, target, occurrence, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, target, occurrence, new Vector2(offset, 0), duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, new Vector2(offset, 0), duration, easing, delay));
                    break;
            }
        }

        public void TransitionToPosition(AnimationTarget target, int occurrence, Vector2 offset, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SavePosition(target, 0);

                    _monoBehaviour.StartCoroutine(TransitionTo(_panelTransform, target, occurrence, offset, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_textComponent[occurrence].rectTransform, target, occurrence, offset, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo(_imageComponent[occurrence].rectTransform, target, occurrence, offset, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SavePosition(target, occurrence);

                    _monoBehaviour.StartCoroutine(TransitionTo((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, offset, duration, easing, delay));
                    break;
            }
        }


        // ----------------------------------------------------- FROM TRANSITION -----------------------------------------------------

        private IEnumerator TransitionFrom(RectTransform component, Vector2 offset, float duration, EasingType easing, float delay)
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

        private IEnumerator TransitionTo(RectTransform component, AnimationTarget target, int occurrence, Vector2 offset, float duration, EasingType easing, float delay)
        {
            GetStartPos(target, occurrence, out Vector2 startPos);
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

        private void GetStartPos(AnimationTarget target, int occurrence, out Vector2 startPos)
        {
            startPos = Vector2.zero;

            switch (target)
            {
                case AnimationTarget.Panel:
                    startPos = _panelTransform.anchoredPosition;
                    break;
                case AnimationTarget.Text:
                    startPos = _textComponent[occurrence].rectTransform.anchoredPosition;
                    break;
                case AnimationTarget.Image:
                    startPos = _imageComponent[occurrence].rectTransform.anchoredPosition;
                    break;
                case AnimationTarget.Button:
                    startPos = ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition;
                    break;
            }
        }

        private void SavePosition(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedPosition[FlowKitConstants.PanelIndex][0])
                    {
                        _originalPosition[FlowKitConstants.PanelIndex][0] = _panelTransform.anchoredPosition;
                        _storedPosition[FlowKitConstants.PanelIndex][0] = true;
                    }
                    break;
                case AnimationTarget.Text:
                    if (!_storedPosition[FlowKitConstants.TextIndex][occurrence])
                    {
                        _originalPosition[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.anchoredPosition;
                        _storedPosition[FlowKitConstants.TextIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Image:
                    if (!_storedPosition[FlowKitConstants.ImageIndex][occurrence])
                    {
                        _originalPosition[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.anchoredPosition;
                        _storedPosition[FlowKitConstants.ImageIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Button:
                    if (!_storedPosition[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        _originalPosition[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).anchoredPosition;
                        _storedPosition[FlowKitConstants.ButtonIndex][occurrence] = true;
                    }
                    break;
            }
        }

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
    }
}
