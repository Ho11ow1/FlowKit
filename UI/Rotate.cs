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
* FlowKit - Rotate Animation Component
* Created by Hollow1
* 
* Applies a rotating animation to a UI component
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
    internal class Rotate
    {
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;
        private readonly RectTransform _panelTransform;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly List<Utils.AutoIncreaseList<Quaternion>> originalRotation = new List<Utils.AutoIncreaseList<Quaternion>>()
        {
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> storedRotation = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public Rotate(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void Rotation(AnimationTarget target, int occurrence, float degrees, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    _monoBehaviour.StartCoroutine(RotateUi(_panelTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    _monoBehaviour.StartCoroutine(RotateUi(_textComponent[occurrence].rectTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    _monoBehaviour.StartCoroutine(RotateUi(_imageComponent[occurrence].rectTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    _monoBehaviour.StartCoroutine(RotateUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, degrees, duration, delay, easing));
                    break;
            }
        }

        // ----------------------------------------------------- ROTATE ANIMATION -----------------------------------------------------

        private IEnumerator RotateUi(RectTransform component, int occurrence, float degrees, float duration, float delay, EasingType easing)
        {
            if (component == null) { yield break; }

            Quaternion startRotation = Quaternion.identity; 
            Quaternion targetRotation;

            if (component == _panelTransform)
            {
                if (!storedRotation[FlowKitConstants.PanelIndex][0])
                {
                    startRotation = _panelTransform.localRotation;
                    originalRotation[FlowKitConstants.PanelIndex][0] = startRotation;
                    storedRotation[FlowKitConstants.PanelIndex][0] = true;
                }
                else
                {
                    startRotation = originalRotation[FlowKitConstants.PanelIndex][0];
                }
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedRotation[FlowKitConstants.TextIndex][occurrence])
                {
                    startRotation = _textComponent[occurrence].rectTransform.localRotation;
                    originalRotation[FlowKitConstants.TextIndex][occurrence] = startRotation;
                    storedRotation[FlowKitConstants.TextIndex][occurrence] = true;
                }
                else
                {
                    startRotation = originalRotation[FlowKitConstants.TextIndex][occurrence];
                }
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedRotation[FlowKitConstants.ImageIndex][occurrence])
                {
                    startRotation = _imageComponent[occurrence].rectTransform.localRotation;
                    originalRotation[FlowKitConstants.ImageIndex][occurrence] = startRotation;
                    storedRotation[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                else
                {
                    startRotation = originalRotation[FlowKitConstants.ImageIndex][occurrence];
                }
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedRotation[FlowKitConstants.ButtonIndex][occurrence])
                {
                    startRotation = _buttonComponent[occurrence].transform.localRotation;
                    originalRotation[FlowKitConstants.ButtonIndex][occurrence] = startRotation;
                    storedRotation[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                else
                {
                    startRotation = originalRotation[FlowKitConstants.ButtonIndex][occurrence];
                }
            }

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            targetRotation = Quaternion.Euler(0, 0, degrees);

            float elapsedTime = 0f;
            FlowKitEvents.InvokeRotateStart();

            while (elapsedTime < duration)
            {
                float time = elapsedTime / duration;
                float easedTime = Utils.Easing.SetEasingFunction(time, easing);

                component.localRotation = Quaternion.Lerp(startRotation, targetRotation, easedTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            component.localRotation = targetRotation;
            FlowKitEvents.InvokeRotateEnd();
        }
    }
}