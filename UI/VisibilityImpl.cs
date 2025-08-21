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
    internal class VisibilityImpl
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly CanvasGroup _panelAlpha;
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;

        private readonly List<Utils.AutoIncreaseList<float>> _originalAlpha= new List<Utils.AutoIncreaseList<float>>()
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

        public VisibilityImpl(TextMeshProUGUI[] text, Image[] image, Button[] button, CanvasGroup panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelAlpha = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetVisibility(AnimationTarget target, int occurrence, bool visibility)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveAlpha(_panelAlpha.gameObject, 0);

                    _panelAlpha.alpha = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveAlpha(_textComponent[occurrence].gameObject, occurrence);

                    _textComponent[occurrence].alpha = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveAlpha(_imageComponent[occurrence].gameObject, occurrence);

                    Color imgColor = _imageComponent[occurrence].color;
                    imgColor.a = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    _imageComponent[occurrence].color = imgColor;
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveAlpha(_buttonComponent[occurrence].gameObject, occurrence);

                    Color btnColor = _buttonComponent[occurrence].image.color;
                    btnColor.a = visibility ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;
                    _buttonComponent[occurrence].image.color = btnColor;
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

                    Color imgColor = _imageComponent[occurrence].color;
                    imgColor.a = _originalAlpha[FlowKitConstants.ImageIndex][occurrence];
                    _imageComponent[occurrence].color = imgColor;
                    break;
                case AnimationTarget.Button:
                    if (!_storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved alpha value found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    Color btnColor = _buttonComponent[occurrence].image.color;
                    btnColor.a = _originalAlpha[FlowKitConstants.ButtonIndex][occurrence];
                    _buttonComponent[occurrence].image.color = btnColor;
                    break;
            }
        }

        public void FadeFromTo(AnimationTarget target, int occurrence, float? fromAlpha, float? toAlpha, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(FadeFromToUi(_panelAlpha.gameObject, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeFromToUi(_textComponent[occurrence].gameObject, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeFromToUi(_imageComponent[occurrence].gameObject, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeFromToUi(_buttonComponent[occurrence].gameObject, occurrence, fromAlpha, toAlpha, duration, easing, delay));
                    break;
            }
        }

        // ----------------------------------------------------- FADE ANIMATIONS -----------------------------------------------------

        private IEnumerator FadeFromToUi(GameObject component, int occurrence, float? fromAlpha, float? toAlpha, float duration, EasingType easing, float delay)
        {
            SaveAlpha(component, occurrence);

            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);
                GetAlphaLerping(component, occurrence, fromAlpha, toAlpha, easedTime, out float animationAlpha);

                if (component == _panelAlpha.gameObject)
                {
                    _panelAlpha.alpha = animationAlpha;
                }
                else if (component == _textComponent[occurrence].gameObject)
                {
                    _textComponent[occurrence].alpha = animationAlpha;
                }
                else if (component == _imageComponent[occurrence].gameObject)
                {
                    Color imgColour = _imageComponent[occurrence].color;
                    imgColour.a = animationAlpha;
                    _imageComponent[occurrence].color = imgColour;
                }
                else if (component == _buttonComponent[occurrence].gameObject)
                {
                    Color btnColour = _buttonComponent[occurrence].image.color;
                    btnColour.a = animationAlpha;
                    _buttonComponent[occurrence].image.color = btnColour;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (component == _panelAlpha.gameObject)
            {
                var endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.PanelIndex][0]);

                _panelAlpha.alpha = endAlpha;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                var endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.TextIndex][occurrence]);

                _textComponent[occurrence].alpha = endAlpha;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                var endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.ImageIndex][occurrence]);

                Color imgColor = _imageComponent[occurrence].color;
                imgColor.a = endAlpha;
                _imageComponent[occurrence].color = imgColor;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                var endAlpha = (float)(toAlpha.HasValue ? toAlpha : _originalAlpha[FlowKitConstants.ButtonIndex][occurrence]);

                Color btnColor = _buttonComponent[occurrence].image.color;
                btnColor.a = endAlpha;
                _buttonComponent[occurrence].image.color = btnColor;
            }

            FlowKitEvents.InvokeFadeEnd();
        }


        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private void GetAlphaLerping(GameObject component, int occurrence, float? fromAlpha, float? toAlpha, float easedTime, out float animationAlpha)
        {
            animationAlpha = 0f;
            if (fromAlpha.HasValue && toAlpha.HasValue)
            {
                animationAlpha = Mathf.Lerp(fromAlpha.Value, toAlpha.Value, easedTime);
            }
            else if (fromAlpha.HasValue && !toAlpha.HasValue)
            {
                if (component == _panelAlpha.gameObject)
                {
                    animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.PanelIndex][0], easedTime);
                }
                else if (component == _textComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.TextIndex][occurrence], easedTime);
                }
                else if (component == _imageComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.ImageIndex][occurrence], easedTime);
                }
                else if (component == _buttonComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(fromAlpha.Value, _originalAlpha[FlowKitConstants.ButtonIndex][occurrence], easedTime);
                }
            }
            else if (!fromAlpha.HasValue && toAlpha.HasValue)
            {
                if (component == _panelAlpha.gameObject)
                {
                    animationAlpha = Mathf.Lerp(_panelAlpha.alpha, toAlpha.Value, easedTime);
                }
                else if (component == _textComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(_textComponent[occurrence].alpha, toAlpha.Value, easedTime);
                }
                else if (component == _imageComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(_imageComponent[occurrence].color.a, toAlpha.Value, easedTime);
                }
                else if (component == _buttonComponent[occurrence].gameObject)
                {
                    animationAlpha = Mathf.Lerp(_buttonComponent[occurrence].image.color.a, toAlpha.Value, easedTime);
                }
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

        private void SaveAlpha(GameObject component, int occurrence)
        {
            if (component == _panelAlpha.gameObject)
            {
                if (!_storedAlpha[FlowKitConstants.PanelIndex][0])
                {
                    _originalAlpha[FlowKitConstants.PanelIndex][0] = _panelAlpha.alpha;
                    _storedAlpha[FlowKitConstants.PanelIndex][0] = true;
                }
                return;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!_storedAlpha[FlowKitConstants.TextIndex][occurrence])
                {
                    _originalAlpha[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].alpha;
                    _storedAlpha[FlowKitConstants.TextIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                if (!_storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                {
                    _originalAlpha[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].color.a;
                    _storedAlpha[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                if (!_storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                {
                    _originalAlpha[FlowKitConstants.ButtonIndex][occurrence] = _buttonComponent[occurrence].image.color.a;
                    _storedAlpha[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                return;
            }
        }
    }
}
