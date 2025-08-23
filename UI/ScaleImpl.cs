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
* FlowKit - Scale Animation Component
* Created by Hollow1
* 
* Applies a Scaling animation to a UI component
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
    internal class ScaleImpl
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly RectTransform _panelTransform;
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;

        private readonly List<Utils.AutoIncreaseList<Vector2>> _originalScale = new List<Utils.AutoIncreaseList<Vector2>>()
        {
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> _storedScale = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public ScaleImpl(MonoBehaviour runner, RectTransform panel, TextMeshProUGUI[] text, Image[] image, Button[] button)
        {
            _monoBehaviour = runner;
            _panelTransform = panel;
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetScale(AnimationTarget target, int occurrence, float scale)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveScale(target, 0);

                    _panelTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    _textComponent[occurrence].rectTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    _imageComponent[occurrence].rectTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).localScale = new Vector2(scale, scale);
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedScale[FlowKitConstants.PanelIndex][0])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _panelTransform.localScale = _originalScale[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    if (!_storedScale[FlowKitConstants.TextIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Text component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _textComponent[occurrence].rectTransform.localScale = _originalScale[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    if (!_storedScale[FlowKitConstants.ImageIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Image component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _imageComponent[occurrence].rectTransform.localScale = _originalScale[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    if (!_storedScale[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved scale found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    ((RectTransform)_buttonComponent[occurrence].transform).localScale = _originalScale[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        public void ScaleFromTo(AnimationTarget target, int occurrence, float? fromScale, float? toScale, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveScale(target, 0);

                    _monoBehaviour.StartCoroutine(ScaleFromToUi(_panelTransform, target, occurrence, fromScale, toScale, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    _monoBehaviour.StartCoroutine(ScaleFromToUi(_textComponent[occurrence].rectTransform, target, occurrence, fromScale, toScale, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    _monoBehaviour.StartCoroutine(ScaleFromToUi(_imageComponent[occurrence].rectTransform, target, occurrence, fromScale, toScale, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveScale(target, occurrence);

                    _monoBehaviour.StartCoroutine(ScaleFromToUi((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, fromScale, toScale, duration, easing, delay));
                    break;
            }
        }

        // ----------------------------------------------------- SCALE ANIMATION -----------------------------------------------------

        private IEnumerator ScaleFromToUi(RectTransform component, AnimationTarget target, int occurrence, float? fromScale, float? toScale, float duration, EasingType easing, float delay)
        {
            GetStartScale(target, occurrence, out Vector2 startScale);

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            CorrectScaleBasedOnValues(ref startScale, in fromScale, in toScale, out Vector2 endScale);
            component.localScale = startScale;

            float elapsedTime = 0f;
            FlowKitEvents.InvokeScaleStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);

                component.localScale = Vector2.Lerp(startScale, endScale, easedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            component.localScale = endScale;
            FlowKitEvents.InvokeScaleEnd();
        }

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private void GetStartScale(AnimationTarget target, int occurrence, out Vector2 startScale)
        {
            startScale = Vector2.zero;

            switch (target)
            {
                case AnimationTarget.Panel:
                    startScale = _panelTransform.localScale;
                    break;
                case AnimationTarget.Text:
                    startScale = _textComponent[occurrence].rectTransform.localScale;
                    break;
                case AnimationTarget.Image:
                    startScale = _imageComponent[occurrence].rectTransform.localScale;
                    break;
                case AnimationTarget.Button:
                    startScale = _buttonComponent[occurrence].transform.localScale;
                    break;
            }
        }

        private void CorrectScaleBasedOnValues(ref Vector2 startScale, in float? fromScale, in float? toScale, out Vector2 endScale)
        {
            endScale = startScale;
            if (fromScale.HasValue && toScale.HasValue)
            {
                startScale *= fromScale.Value;
                endScale *= toScale.Value;
            }
            else if (fromScale.HasValue && !toScale.HasValue)
            {
                startScale *= fromScale.Value;
            }
            else if (!fromScale.HasValue && toScale.HasValue)
            {
                endScale *= toScale.Value;
            }
        }

        private void SaveScale(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedScale[FlowKitConstants.PanelIndex][0])
                    {
                        _originalScale[FlowKitConstants.PanelIndex][0] = _panelTransform.localScale;
                        _storedScale[FlowKitConstants.PanelIndex][0] = true;
                    }
                    break;
                case AnimationTarget.Text:
                    if (!_storedScale[FlowKitConstants.TextIndex][occurrence])
                    {
                        _originalScale[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.localScale;
                        _storedScale[FlowKitConstants.TextIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Image:
                    if (!_storedScale[FlowKitConstants.ImageIndex][occurrence])
                    {
                        _originalScale[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.localScale;
                        _storedScale[FlowKitConstants.ImageIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Button:
                    if (!_storedScale[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        _originalScale[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).localScale;
                        _storedScale[FlowKitConstants.ButtonIndex][occurrence] = true;
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
