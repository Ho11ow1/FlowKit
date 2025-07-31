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
using FlowKit.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlowKit.UI
{
    [AddComponentMenu("")]
    internal class ScaleImpl
    {
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;
        private readonly RectTransform _panelTransform;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly List<Utils.AutoIncreaseList<Vector2>> originalScale = new List<Utils.AutoIncreaseList<Vector2>>()
        {
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>(),
            new Utils.AutoIncreaseList<Vector2>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> storedScale = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public ScaleImpl(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetScale(AnimationTarget target, int occurrence, float scale)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveScale(_panelTransform.gameObject, 0);

                    _panelTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveScale(_textComponent[occurrence].gameObject, occurrence);

                    _textComponent[occurrence].rectTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveScale(_imageComponent[occurrence].gameObject, occurrence);

                    _imageComponent[occurrence].rectTransform.localScale = new Vector2(scale, scale);
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveScale(_buttonComponent[occurrence].gameObject, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).localScale = new Vector2(scale, scale);
                    break;
            }
        }

        public void ScaleUp(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_panelTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_textComponent[occurrence].rectTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_imageComponent[occurrence].rectTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, multiplier, duration, delay, easing));
                    break;
            }
        }

        public void ScaleDown(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_panelTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_textComponent[occurrence].rectTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi(_imageComponent[occurrence].rectTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(ScaleUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    #if UNITY_EDITOR
                    if (!storedScale[FlowKitConstants.PanelIndex][0])
                    {
                        Debug.LogError($"No saved scale found for Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _panelTransform.localScale = originalScale[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    #if UNITY_EDITOR
                    if (!storedScale[FlowKitConstants.TextIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Text component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _textComponent[occurrence].rectTransform.localScale = originalScale[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    #if UNITY_EDITOR
                    if (!storedScale[FlowKitConstants.ImageIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Image component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _imageComponent[occurrence].rectTransform.localScale = originalScale[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    #if UNITY_EDITOR
                    if (!storedScale[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        Debug.LogError($"No saved scale found for Button component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    ((RectTransform)_buttonComponent[occurrence].transform).localScale = originalScale[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        // ----------------------------------------------------- SCALE ANIMATION -----------------------------------------------------

        private IEnumerator ScaleUi(RectTransform component, int occurrence, float scaleAmount, float duration, float delay, EasingType easing)
        {
            GetStartScale(component, occurrence, out Vector2 startScale);
            Vector2 targetScale;

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            targetScale = startScale * scaleAmount;

            float elapsedTime = 0f;
            FlowKitEvents.InvokeScaleStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);

                component.localScale = Vector2.Lerp(startScale, targetScale, easedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            component.localScale = targetScale;
            FlowKitEvents.InvokeScaleEnd();
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

        private void SaveScale(GameObject component, int occurrence)
        {
            if (component == _panelTransform.gameObject)
            {
                if (!storedScale[FlowKitConstants.PanelIndex][0])
                {
                    originalScale[FlowKitConstants.PanelIndex][0] = _panelTransform.localScale;
                    storedScale[FlowKitConstants.PanelIndex][0] = true;
                }
                return;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedScale[FlowKitConstants.TextIndex][occurrence])
                {
                    originalScale[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.localScale;
                    storedScale[FlowKitConstants.TextIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                if (!storedScale[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalScale[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.localScale;
                    storedScale[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                if (!storedScale[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalScale[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).localScale;
                    storedScale[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                return;
            }
        }

        private void GetStartScale(RectTransform component, int occurrence, out Vector2 startScale)
        {
            startScale = Vector2.zero;

            if (component == _panelTransform)
            {
                if (!storedScale[FlowKitConstants.PanelIndex][0])
                {
                    SaveScale(_panelTransform.gameObject, 0);
                }
                startScale = _panelTransform.localScale;
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedScale[FlowKitConstants.TextIndex][occurrence])
                {
                    SaveScale(_panelTransform.gameObject, occurrence);
                }
                startScale = _textComponent[occurrence].rectTransform.localScale;
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedScale[FlowKitConstants.ImageIndex][occurrence])
                {
                    SaveScale(_panelTransform.gameObject, occurrence);
                }
                startScale = _imageComponent[occurrence].rectTransform.localScale;
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedScale[FlowKitConstants.ButtonIndex][occurrence])
                {
                    SaveScale(_panelTransform.gameObject, occurrence);
                }
                startScale = _buttonComponent[occurrence].transform.localScale;
            }
        }
    }
}
