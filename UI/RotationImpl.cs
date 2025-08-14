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
    internal class RotationImpl
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

        public RotationImpl(TextMeshProUGUI[] text, Image[] image, Button[] button, RectTransform panel, MonoBehaviour runner)
        {
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
            _panelTransform = panel;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetRotation(AnimationTarget target, int occurrence, float degrees)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveRotation(_panelTransform.gameObject, 0);

                    _panelTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveRotation(_textComponent[occurrence].gameObject, occurrence);

                    _textComponent[occurrence].rectTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveRotation(_imageComponent[occurrence].gameObject, occurrence);

                    _imageComponent[occurrence].rectTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveRotation(_buttonComponent[occurrence].gameObject, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
            }
        }

        public void Rotation(AnimationTarget target, int occurrence, float degrees, EasingType easing = EasingType.Linear, float duration = FlowKitConstants.DefaultDuration, float delay = 0f)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUi(_panelTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUi(_textComponent[occurrence].rectTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUi(_imageComponent[occurrence].rectTransform, occurrence, degrees, duration, delay, easing));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUi((RectTransform)_buttonComponent[occurrence].transform, occurrence, degrees, duration, delay, easing));
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!storedRotation[FlowKitConstants.PanelIndex][0]) 
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _panelTransform.localRotation = originalRotation[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    if (!storedRotation[FlowKitConstants.TextIndex][occurrence]) 
                    { 
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Text component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _textComponent[occurrence].rectTransform.localRotation = originalRotation[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    if (!storedRotation[FlowKitConstants.ImageIndex][occurrence]) 
                    { 
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Image component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _imageComponent[occurrence].rectTransform.localRotation = originalRotation[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    if (!storedRotation[FlowKitConstants.ButtonIndex][occurrence]) 
                    { 
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    ((RectTransform)_buttonComponent[occurrence].transform).localRotation = originalRotation[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        // ----------------------------------------------------- ROTATE ANIMATION -----------------------------------------------------

        private IEnumerator RotateUi(RectTransform component, int occurrence, float degrees, float duration, float delay, EasingType easing)
        {
            GetStartRotation(component, occurrence, out Quaternion startRotation); 
            Quaternion targetRotation;

            if (delay > 0) { yield return new WaitForSeconds(delay); }
            float currentZ = startRotation.eulerAngles.z;
            targetRotation = Quaternion.Euler(0, 0, currentZ + degrees);

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

        private void SaveRotation(GameObject component, int occurrence)
        {
            if (component == _panelTransform.gameObject)
            {
                if (!storedRotation[FlowKitConstants.PanelIndex][0])
                {
                    originalRotation[FlowKitConstants.PanelIndex][0] = _panelTransform.localRotation;
                    storedRotation[FlowKitConstants.PanelIndex][0] = true;
                }
                return;
            }
            else if (component == _textComponent[occurrence].gameObject)
            {
                if (!storedRotation[FlowKitConstants.TextIndex][occurrence])
                {
                    originalRotation[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.localRotation;
                    storedRotation[FlowKitConstants.TextIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _imageComponent[occurrence].gameObject)
            {
                if (!storedRotation[FlowKitConstants.ImageIndex][occurrence])
                {
                    originalRotation[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.localRotation;
                    storedRotation[FlowKitConstants.ImageIndex][occurrence] = true;
                }
                return;
            }
            else if (component == _buttonComponent[occurrence].gameObject)
            {
                if (!storedRotation[FlowKitConstants.ButtonIndex][occurrence])
                {
                    originalRotation[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).localRotation;
                    storedRotation[FlowKitConstants.ButtonIndex][occurrence] = true;
                }
                return;
            }
        }

        private void GetStartRotation(RectTransform component, int occurrence, out Quaternion startRotation)
        {
            startRotation = Quaternion.identity;

            if (component == _panelTransform)
            {
                if (!storedRotation[FlowKitConstants.PanelIndex][0])
                {
                    SaveRotation(_panelTransform.gameObject, 0);
                }
                startRotation = _panelTransform.localRotation;
            }
            else if (component == _textComponent[occurrence].rectTransform)
            {
                if (!storedRotation[FlowKitConstants.TextIndex][occurrence])
                {
                    SaveRotation(_textComponent[occurrence].gameObject, occurrence);
                }
                startRotation = _textComponent[occurrence].rectTransform.localRotation;
            }
            else if (component == _imageComponent[occurrence].rectTransform)
            {
                if (!storedRotation[FlowKitConstants.ImageIndex][occurrence])
                {
                    SaveRotation(_imageComponent[occurrence].gameObject, occurrence);
                }
                startRotation = _imageComponent[occurrence].rectTransform.localRotation;
            }
            else if (component == (RectTransform)_buttonComponent[occurrence].transform)
            {
                if (!storedRotation[FlowKitConstants.ButtonIndex][occurrence])
                {
                    SaveRotation(_buttonComponent[occurrence].gameObject, occurrence);
                }
                startRotation = _buttonComponent[occurrence].transform.localRotation;
            }
        }
    }
}
