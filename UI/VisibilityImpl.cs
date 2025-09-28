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
* FlowKit - Visibility Animation Component
* Created by Hollow1
* 
* Applies visibility based animations to a UI component
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
    internal class VisibilityImpl
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly CanvasGroup _panelAlpha;
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;
        private readonly TextMeshProUGUI[] _buttonTexts;

        private readonly List<Utils.AutoIncreaseList<float>> _originalAlpha = new List<Utils.AutoIncreaseList<float>>()
        {
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> _storedAlpha = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public VisibilityImpl(MonoBehaviour runner, CanvasGroup panel, TextMeshProUGUI[] text, Image[] image, Button[] button, TextMeshProUGUI[] buttonTexts)
        {
            _monoBehaviour = runner;
            _panelAlpha = panel;
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _buttonTexts = buttonTexts;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetVisibility(AnimationTarget target, int occurrence, bool visibility)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveAlpha(target, 0);

                    _panelAlpha.alpha = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    _textComponent[occurrence].alpha = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    var imgComponent = _imageComponent[occurrence];

                    Color imgColor = imgComponent.color;
                    imgColor.a = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    imgComponent.color = imgColor;
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    var btnComponent = _buttonComponent[occurrence];
                    var alpha = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;

                    Color btnColor = btnComponent.image.color;
                    btnColor.a = alpha;
                    btnComponent.image.color = btnColor;

                    if (IsButtonTextValid(occurrence)) { _buttonTexts[occurrence].alpha = alpha; }
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedAlpha[FlowKitConstants.PanelIndex][0])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved alpha value found for Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _panelAlpha.alpha = _originalAlpha[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    if (!_storedAlpha[FlowKitConstants.TextIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved alpha value found for Text component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _textComponent[occurrence].alpha = _originalAlpha[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    if (!_storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved alpha value found for Image component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    var imgComponent = _imageComponent[occurrence];

                    Color imgColor = imgComponent.color;
                    imgColor.a = _originalAlpha[FlowKitConstants.ImageIndex][occurrence];
                    imgComponent.color = imgColor;
                    break;
                case AnimationTarget.Button:
                    if (!_storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved alpha value found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    var btnComponent = _buttonComponent[occurrence];

                    Color btnColor = btnComponent.image.color;
                    btnColor.a = _originalAlpha[FlowKitConstants.ButtonIndex][occurrence];
                    btnComponent.image.color = btnColor;

                    if (IsButtonTextValid(occurrence)) { _buttonTexts[occurrence].alpha = FlowKitConstants.OpaqueAlpha; }
                    break;
            }
        }

        public void FadeFromTo(AnimationTarget target, int occurrence, float? fromAlpha, float? toAlpha, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveAlpha(target, 0);

                    _monoBehaviour.StartCoroutine(FadeFromToUi(AnimationTarget.Panel, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    _monoBehaviour.StartCoroutine(FadeFromToUi(AnimationTarget.Text, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    _monoBehaviour.StartCoroutine(FadeFromToUi(AnimationTarget.Image, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveAlpha(target, occurrence);

                    _monoBehaviour.StartCoroutine(FadeFromToUi(AnimationTarget.Button, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
            }
        }

        // ----------------------------------------------------- FADE ANIMATIONS -----------------------------------------------------

        private IEnumerator FadeFromToUi(AnimationTarget target, int occurrence, float? fromAlpha, float? toAlpha, float duration, EasingType easing, float delay)
        {
            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);
                GetAlphaLerping(target, occurrence, fromAlpha, toAlpha, easedTime, out float animationAlpha);

                switch (target)
                {
                    case AnimationTarget.Panel:
                        _panelAlpha.alpha = animationAlpha;
                        break;
                    case AnimationTarget.Text:
                        _textComponent[occurrence].alpha = animationAlpha;
                        break;
                    case AnimationTarget.Image:
                        var imgComponent = _imageComponent[occurrence];

                        Color imgColour = imgComponent.color;
                        imgColour.a = animationAlpha;
                        imgComponent.color = imgColour;
                        break;
                    case AnimationTarget.Button:
                        var btnComponent = _buttonComponent[occurrence];

                        Color btnColour = btnComponent.image.color;
                        btnColour.a = animationAlpha;
                        btnComponent.image.color = btnColour;
                        break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            var endAlpha = 0f;

            switch (target)
            {
                case AnimationTarget.Panel:
                    endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.PanelIndex][0]);

                    _panelAlpha.alpha = endAlpha;
                    break;
                case AnimationTarget.Text:
                    endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.TextIndex][occurrence]);

                    _textComponent[occurrence].alpha = endAlpha;
                    break;
                case AnimationTarget.Image:
                    endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.ImageIndex][occurrence]);
                    var imgComponent = _imageComponent[occurrence];

                    Color imgColor = imgComponent.color;
                    imgColor.a = endAlpha;
                    imgComponent.color = imgColor;
                    break;
                case AnimationTarget.Button:
                    endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.ButtonIndex][occurrence]);
                    var btnComponent = _buttonComponent[occurrence];

                    Color btnColor = btnComponent.image.color;
                    btnColor.a = endAlpha;
                    btnComponent.image.color = btnColor;
                    break;
            }

            FlowKitEvents.InvokeFadeEnd();
        }


        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private void GetAlphaLerping(AnimationTarget target, int occurrence, float? fromAlpha, float? toAlpha, float easedTime, out float animationAlpha)
        {
            animationAlpha = 0f;
            if (fromAlpha.HasValue && toAlpha.HasValue)
            {
                animationAlpha = Mathf.Lerp(fromAlpha.Value, toAlpha.Value, easedTime);
            }
            else if (fromAlpha.HasValue && !toAlpha.HasValue)
            {
                switch (target)
                {
                    case AnimationTarget.Panel:
                        animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.PanelIndex][0], easedTime);
                        break;
                    case AnimationTarget.Text:
                        animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.TextIndex][occurrence], easedTime);
                        break;
                    case AnimationTarget.Image:
                        animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.ImageIndex][occurrence], easedTime);
                        break;
                    case AnimationTarget.Button:
                        animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.ButtonIndex][occurrence], easedTime);
                        break;
                }
            }
            else if (!fromAlpha.HasValue && toAlpha.HasValue)
            {
                switch (target)
                {
                    case AnimationTarget.Panel:
                        animationAlpha = Mathf.Lerp(_panelAlpha.alpha, toAlpha.Value, easedTime);
                        break;
                    case AnimationTarget.Text:
                        animationAlpha = Mathf.Lerp(_textComponent[occurrence].alpha, toAlpha.Value, easedTime);
                        break;
                    case AnimationTarget.Image:
                        animationAlpha = Mathf.Lerp(_imageComponent[occurrence].color.a, toAlpha.Value, easedTime);
                        break;
                    case AnimationTarget.Button:
                        animationAlpha = Mathf.Lerp(_buttonComponent[occurrence].image.color.a, toAlpha.Value, easedTime);
                        break;
                }
            }
        }

        private void SaveAlpha(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedAlpha[FlowKitConstants.PanelIndex][0])
                    {
                        _originalAlpha[FlowKitConstants.PanelIndex][0] = _panelAlpha.alpha;
                        _storedAlpha[FlowKitConstants.PanelIndex][0] = true;
                    }
                    break;
                case AnimationTarget.Text:
                    if (!_storedAlpha[FlowKitConstants.TextIndex][occurrence])
                    {
                        _originalAlpha[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].alpha;
                        _storedAlpha[FlowKitConstants.TextIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Image:
                    if (!_storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                    {
                        _originalAlpha[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].color.a;
                        _storedAlpha[FlowKitConstants.ImageIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Button:
                    if (!_storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        _originalAlpha[FlowKitConstants.ButtonIndex][occurrence] = _buttonComponent[occurrence].image.color.a;
                        _storedAlpha[FlowKitConstants.ButtonIndex][occurrence] = true;
                    }
                    break;
            }
        }

        private bool IndexNullChecksPass(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    return _panelAlpha != null;
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

        private bool IsButtonTextValid(int occurrence)
        {
            return occurrence < _buttonTexts.Length;
        }
    }
}
