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
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;
        private readonly CanvasGroup _panelAlpha;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly List<Utils.AutoIncreaseList<float>> originalAlpha= new List<Utils.AutoIncreaseList<float>>()
        {
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>(),
            new Utils.AutoIncreaseList<float>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> storedAlpha = new List<Utils.AutoIncreaseList<bool>>()
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

        public void FadeIn(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_panelAlpha.gameObject, occurrence, duration, delay, true));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_textComponent[occurrence].gameObject, occurrence, duration, delay, true));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_imageComponent[occurrence].gameObject, occurrence, duration, delay, true));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_buttonComponent[occurrence].gameObject, occurrence, duration, delay, true));
                    break;
            }
        }

        public void FadeOut(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_panelAlpha.gameObject, occurrence, duration, delay, false));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_textComponent[occurrence].gameObject, occurrence, duration, delay, false));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_imageComponent[occurrence].gameObject, occurrence, duration, delay, false));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeInOut(_buttonComponent[occurrence].gameObject, occurrence, duration, delay, false));
                    break;
            }
        }

        public void FadeTo(AnimationTarget target, int occurrence, float alpha, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(FadeUiTo(_panelAlpha.gameObject, occurrence, alpha, duration, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeUiTo(_textComponent[occurrence].gameObject, occurrence, alpha, duration, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeUiTo(_imageComponent[occurrence].gameObject, occurrence, alpha, duration, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(FadeUiTo(_buttonComponent[occurrence].gameObject, occurrence, alpha, duration, delay));
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    #if UNITY_EDITOR
                    if (!storedAlpha[FlowKitConstants.PanelIndex][0])
                    {
                        Debug.LogError($"No saved alpha value found for Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _panelAlpha.alpha= originalAlpha[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    #if UNITY_EDITOR
                    if (!storedAlpha[FlowKitConstants.TextIndex][occurrence])
                    {
                        Debug.LogError($"No saved alpha value found for Text component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    _textComponent[occurrence].alpha = originalAlpha[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    #if UNITY_EDITOR
                    if (!storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                    {
                        Debug.LogError($"No saved alpha value found for Image component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    Color imgColor = _imageComponent[occurrence].color;
                    imgColor.a = originalAlpha[FlowKitConstants.ImageIndex][occurrence];
                    _imageComponent[occurrence].color = imgColor;
                    break;
                case AnimationTarget.Button:
                    #if UNITY_EDITOR
                    if (!storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        Debug.LogError($"No saved alpha value found for Button component child. Panel: [{gameObject.name}]");
                        return;
                    }
                    #endif

                    Color btnColor = _buttonComponent[occurrence].image.color;
                    btnColor.a = originalAlpha[FlowKitConstants.ButtonIndex][occurrence];
                    _buttonComponent[occurrence].image.color = btnColor;
                    break;
            }
        }

        // ----------------------------------------------------- FADE ANIMATIONS -----------------------------------------------------

        private IEnumerator FadeInOut(GameObject component, int occurrence, float duration, float delay, bool fadeIn)
        {
            SaveAlpha(component, occurrence);

            if (component == _panelAlpha.gameObject)
            {
                _panelAlpha.alpha = fadeIn ? FlowKitConstants.TransparentAlpha : FlowKitConstants.OpaqueAlpha;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                _textComponent[occurrence].alpha = fadeIn ? FlowKitConstants.TransparentAlpha : FlowKitConstants.OpaqueAlpha;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColor = _imageComponent[occurrence].color;
                imgColor.a = fadeIn ? FlowKitConstants.TransparentAlpha : FlowKitConstants.OpaqueAlpha;
                _imageComponent[occurrence].color = imgColor;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColor = _buttonComponent[occurrence].image.color;
                btnColor.a = fadeIn ? FlowKitConstants.TransparentAlpha : FlowKitConstants.OpaqueAlpha;
                _buttonComponent[occurrence].image.color = btnColor;
            }

            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float animationAlpha = fadeIn ? 
                    Mathf.Lerp(FlowKitConstants.TransparentAlpha, FlowKitConstants.OpaqueAlpha, time) 
                    : 
                    Mathf.Lerp(FlowKitConstants.OpaqueAlpha, FlowKitConstants.TransparentAlpha, time);

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

            float endAlpha = fadeIn ? FlowKitConstants.OpaqueAlpha : FlowKitConstants.TransparentAlpha;

            if (component == _panelAlpha.gameObject)
            {
                _panelAlpha.alpha = endAlpha;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                _textComponent[occurrence].alpha = endAlpha;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColor = _imageComponent[occurrence].color;
                imgColor.a = endAlpha;
                _imageComponent[occurrence].color = imgColor;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColor = _buttonComponent[occurrence].image.color;
                btnColor.a = endAlpha;
                _buttonComponent[occurrence].image.color = btnColor;
            }

            FlowKitEvents.InvokeFadeEnd();
        }

        private IEnumerator FadeUiTo(GameObject component, int occurrence, float targetAlpha, float duration, float delay)
        {
            SaveAlpha(component, occurrence);

            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;

                if (component == _panelAlpha.gameObject)
                {
                    _panelAlpha.alpha = Mathf.Lerp(_panelAlpha.alpha, targetAlpha, time);
                }
                else if (component == _textComponent[occurrence].gameObject)
                {
                    _textComponent[occurrence].alpha = Mathf.Lerp(_textComponent[occurrence].alpha, targetAlpha, time);
                }
                else if (component == _imageComponent[occurrence].gameObject)
                {
                    Color imgColour = _imageComponent[occurrence].color;
                    imgColour.a = Mathf.Lerp(imgColour.a, targetAlpha, time);
                    _imageComponent[occurrence].color = imgColour;
                }
                else if (component == _buttonComponent[occurrence].gameObject)
                {
                    Color btnColour = _buttonComponent[occurrence].image.color;
                    btnColour.a = Mathf.Lerp(btnColour.a, targetAlpha, time);
                    _buttonComponent[occurrence].image.color = btnColour;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (component == _panelAlpha.gameObject)
            {
                _panelAlpha.alpha = targetAlpha;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                _textComponent[occurrence].alpha = targetAlpha;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColor = _imageComponent[occurrence].color;
                imgColor.a = targetAlpha;
                _imageComponent[occurrence].color = imgColor;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColour = _buttonComponent[occurrence].image.color;
                btnColour.a = targetAlpha;
                _buttonComponent[occurrence].image.color = btnColour;
            }

            FlowKitEvents.InvokeFadeEnd();
        }

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

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
                if (!storedAlpha[FlowKitConstants.PanelIndex][0])
                {
                    originalAlpha[FlowKitConstants.PanelIndex][0] = _panelAlpha.alpha;
                    storedAlpha[FlowKitConstants.PanelIndex][0] = true;
                }
                return;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedAlpha[FlowKitConstants.TextIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].alpha;
                    storedAlpha[FlowKitConstants.TextIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                if (!storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].color.a;
                    storedAlpha[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                if (!storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ButtonIndex][occurrence] = _buttonComponent[occurrence].image.color.a;
                    storedAlpha[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                return;
            }
        }
    }
}