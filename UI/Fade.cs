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
* FlowKit - Fade Animation Component
* Created by Hollow1
* 
* Applies a Fade animation to a UI component
* 
* Version: 2.4.0
* GitHub: https://github.com/Hollow1/FlowKit
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
    internal class Fade
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


        private const float transparent = 0f;
        private const float visible = 1.0f;

        public Fade(TextMeshProUGUI[] text, Image[] image, Button[] button, CanvasGroup panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelAlpha = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetPanelVisibility(bool visibility)
        {
            if (_panelAlpha == null) { return; }

            _panelAlpha.alpha = visibility ? visible : transparent;
        }

        public void FadeIn(AnimationTarget target, int occurrence, float delay = 0f, float duration = FlowKitConstants.DefaultDuration)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(FadeUiIn(_panelAlpha.gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(FadeUiIn(_textComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(FadeUiIn(_imageComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(FadeUiIn(_buttonComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
            }
        }

        public void FadeOut(AnimationTarget target, int occurrence, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(FadeUiOut(_panelAlpha.gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(FadeUiOut(_textComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(FadeUiOut(_imageComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(FadeUiOut(_buttonComponent[occurrence].gameObject, occurrence, duration, delay));
                    break;
            }
        }

        // ----------------------------------------------------- FADE IN ANIMATION -----------------------------------------------------

        private IEnumerator FadeUiIn(GameObject component, int occurrence, float duration, float delay)
        {
            if (component == null) { yield break; }

            if (component == _panelAlpha.gameObject)
            {
                if (!storedAlpha[FlowKitConstants.PanelIndex][0])
                {
                    originalAlpha[FlowKitConstants.PanelIndex][0] = _panelAlpha.alpha;
                    storedAlpha[FlowKitConstants.PanelIndex][0] = true;
                }
                _panelAlpha.alpha = transparent;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedAlpha[FlowKitConstants.TextIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].alpha;
                    storedAlpha[FlowKitConstants.TextIndex][occurrence] = true;
                }
                _textComponent[occurrence].alpha = transparent;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColour = _imageComponent[occurrence].color;
                if (!storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ImageIndex][occurrence] = imgColour.a;
                    storedAlpha[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                imgColour.a = transparent;
                _imageComponent[occurrence].color = imgColour;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColour = _buttonComponent[occurrence].image.color;
                if (!storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ButtonIndex][occurrence] = btnColour.a;
                    storedAlpha[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                btnColour.a = transparent;
                _buttonComponent[occurrence].image.color = btnColour;
            }

            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;

                if (component == _panelAlpha.gameObject) 
                { 
                    _panelAlpha.alpha = Mathf.Lerp(transparent, visible, time); 
                }
                else if (component == _textComponent[occurrence].gameObject) 
                { 
                    _textComponent[occurrence].alpha = Mathf.Lerp(transparent, visible, time); 
                }
                else if (component == _imageComponent[occurrence].gameObject) 
                {
                    Color imgColour = _imageComponent[occurrence].color;
                    imgColour.a = Mathf.Lerp(transparent, visible, time);
                    _imageComponent[occurrence].color = imgColour;
                }
                else if (component == _buttonComponent[occurrence].gameObject) 
                { 
                    Color btnColour = _buttonComponent[occurrence].image.color;
                    btnColour.a = Mathf.Lerp(transparent, visible, time);
                    _buttonComponent[occurrence].image.color = btnColour;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (component == _panelAlpha.gameObject) 
            {
                _panelAlpha.alpha = visible; 
            }
            else if (component == _textComponent[occurrence].gameObject) 
            { 
                _textComponent[occurrence].alpha = visible; 
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColour = _imageComponent[occurrence].color;
                imgColour.a = visible;
                _imageComponent[occurrence].color = imgColour;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColour = _buttonComponent[occurrence].image.color;
                btnColour.a = visible;
                _buttonComponent[occurrence].image.color = btnColour;
            }

            FlowKitEvents.InvokeFadeEnd();
        }

        // ----------------------------------------------------- FADE OUT ANIMATION -----------------------------------------------------

        private IEnumerator FadeUiOut(GameObject component, int occurrence, float duration, float delay)
        {
            if (component == null) { yield break; }

            if (component == _panelAlpha.gameObject)
            {
                if (!storedAlpha[FlowKitConstants.PanelIndex][0])
                {
                    originalAlpha[FlowKitConstants.PanelIndex][0] = _panelAlpha.alpha;
                    storedAlpha[FlowKitConstants.PanelIndex][0] = true;
                }
                _panelAlpha.alpha = visible;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedAlpha[FlowKitConstants.TextIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].alpha;
                    storedAlpha[FlowKitConstants.TextIndex][occurrence] = true;
                }
                _textComponent[occurrence].alpha = visible;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColour = _imageComponent[occurrence].color;
                if (!storedAlpha[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ImageIndex][occurrence] = imgColour.a;
                    storedAlpha[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                imgColour.a = visible;
                _imageComponent[occurrence].color = imgColour;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColour = _buttonComponent[occurrence].image.color;
                if (!storedAlpha[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalAlpha[FlowKitConstants.ButtonIndex][occurrence] = btnColour.a;
                    storedAlpha[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                btnColour.a = visible;
                _buttonComponent[occurrence].image.color = btnColour;
            }

            if (delay > 0) { yield return new WaitForSeconds(delay); }

            float elapsedTime = 0f;
            FlowKitEvents.InvokeFadeStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;

                if (component == _panelAlpha.gameObject)
                {
                    _panelAlpha.alpha = Mathf.Lerp(visible, transparent, time);
                }
                else if (component == _textComponent[occurrence].gameObject)
                {
                    _textComponent[occurrence].alpha = Mathf.Lerp(visible, transparent, time);
                }
                else if (component == _imageComponent[occurrence].gameObject)
                {
                    Color imgColour = _imageComponent[occurrence].color;
                    imgColour.a = Mathf.Lerp(visible, transparent, time);
                    _imageComponent[occurrence].color = imgColour;
                }
                else if (component == _buttonComponent[occurrence].gameObject)
                {
                    Color btnColour = _buttonComponent[occurrence].image.color;
                    btnColour.a = Mathf.Lerp(visible, transparent, time);
                    _buttonComponent[occurrence].image.color = btnColour;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (component == _panelAlpha.gameObject)
            {
                _panelAlpha.alpha = transparent;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                _textComponent[occurrence].alpha = transparent;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                Color imgColour = _imageComponent[occurrence].color;
                imgColour.a = transparent;
                _imageComponent[occurrence].color = imgColour;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                Color btnColour = _buttonComponent[occurrence].image.color;
                btnColour.a = transparent;
                _buttonComponent[occurrence].image.color = btnColour;
            }

            FlowKitEvents.InvokeFadeEnd();
        }
    }
}