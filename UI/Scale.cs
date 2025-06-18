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
* Version: 1.0.0
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
    internal class Scale
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

        public Scale(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void ScaleUp(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(ScaleUi(_panelTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(ScaleUi(_textComponent[occurrence].rectTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(ScaleUi(_imageComponent[occurrence].rectTransform, occurrence, multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(ScaleUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, multiplier, duration, delay, easing));
                    break;
            }
        }

        public void ScaleDown(AnimationTarget target, int occurrence, float multiplier, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(ScaleUi(_panelTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(ScaleUi(_textComponent[occurrence].rectTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(ScaleUi(_imageComponent[occurrence].rectTransform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(ScaleUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, 1 / multiplier, duration, delay, easing));
                    break;
            }
        }

        // ----------------------------------------------------- SCALE ANIMATION -----------------------------------------------------

        private IEnumerator ScaleUi(RectTransform component, int occurrence, float scaleAmount, float duration, float delay, EasingType easing)
        {
            if (component == null) { yield break; }

            Vector2 startScale = Vector2.zero;
            Vector2 targetScale;


            if (component == _panelTransform)
            {
                if (!storedScale[FlowKitConstants.PanelIndex][0])
                {
                    startScale = _panelTransform.localScale;
                    originalScale[FlowKitConstants.PanelIndex][0] = startScale;
                    storedScale[FlowKitConstants.PanelIndex][0] = true;
                }
                else
                {
                    startScale = originalScale[FlowKitConstants.PanelIndex][0];
                }
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedScale[FlowKitConstants.TextIndex][occurrence])
                {
                    startScale = _textComponent[occurrence].rectTransform.localScale;
                    originalScale[FlowKitConstants.TextIndex][occurrence] = startScale;
                    storedScale[FlowKitConstants.TextIndex][occurrence] = true;
                }
                else
                {
                    startScale = originalScale[FlowKitConstants.TextIndex][occurrence];
                }
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedScale[FlowKitConstants.ImageIndex][occurrence])
                {
                    startScale = _imageComponent[occurrence].rectTransform.localScale;
                    originalScale[FlowKitConstants.ImageIndex][occurrence] = startScale;
                    storedScale[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                else
                {
                    startScale = originalScale[FlowKitConstants.ImageIndex][occurrence];
                }
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedScale[FlowKitConstants.ButtonIndex][occurrence])
                {
                    startScale = _buttonComponent[occurrence].transform.localScale;
                    originalScale[FlowKitConstants.ButtonIndex][occurrence] = startScale;
                    storedScale[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                else
                {
                    startScale = originalScale[FlowKitConstants.ButtonIndex][occurrence];
                }
            }

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
    }
}