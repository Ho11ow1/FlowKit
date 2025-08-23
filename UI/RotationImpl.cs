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
        private readonly MonoBehaviour _monoBehaviour;
        private readonly RectTransform _panelTransform;
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly Image[] _imageComponent;
        private readonly Button[] _buttonComponent;

        private readonly List<Utils.AutoIncreaseList<Quaternion>> _originalRotation = new List<Utils.AutoIncreaseList<Quaternion>>()
        {
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>(),
            new Utils.AutoIncreaseList<Quaternion>()
        };

        private readonly List<Utils.AutoIncreaseList<bool>> _storedRotation = new List<Utils.AutoIncreaseList<bool>>()
        {
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>(),
            new Utils.AutoIncreaseList<bool>()
        };

        public RotationImpl(MonoBehaviour runner, RectTransform panel, TextMeshProUGUI[] text, Image[] image, Button[] button)
        {
            _monoBehaviour = runner;
            _panelTransform = panel;
            _textComponent = text;
            _imageComponent = image;
            _buttonComponent = button;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void SetRotation(AnimationTarget target, int occurrence, float degrees)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveRotation(target, 0);

                    _panelTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    _textComponent[occurrence].rectTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    _imageComponent[occurrence].rectTransform.localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    ((RectTransform)_buttonComponent[occurrence].transform).localRotation = Quaternion.Euler(0, 0, degrees);
                    break;
            }
        }

        public void Reset(AnimationTarget target, int occurrence, GameObject gameObject)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedRotation[FlowKitConstants.PanelIndex][0])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _panelTransform.localRotation = _originalRotation[FlowKitConstants.PanelIndex][0];
                    break;
                case AnimationTarget.Text:
                    if (!_storedRotation[FlowKitConstants.TextIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Text component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _textComponent[occurrence].rectTransform.localRotation = _originalRotation[FlowKitConstants.TextIndex][occurrence];
                    break;
                case AnimationTarget.Image:
                    if (!_storedRotation[FlowKitConstants.ImageIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Image component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    _imageComponent[occurrence].rectTransform.localRotation = _originalRotation[FlowKitConstants.ImageIndex][occurrence];
                    break;
                case AnimationTarget.Button:
                    if (!_storedRotation[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        #if UNITY_EDITOR
                        Debug.LogError($"No saved rotation found for Button component child. Panel: [{gameObject.name}]");
                        #endif
                        return;
                    }

                    ((RectTransform)_buttonComponent[occurrence].transform).localRotation = _originalRotation[FlowKitConstants.ButtonIndex][occurrence];
                    break;
            }
        }

        public void Rotation(AnimationTarget target, int occurrence, float degrees, float duration, EasingType easing, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    SaveRotation(target, 0);

                    _monoBehaviour.StartCoroutine(RotateUi(_panelTransform, target, occurrence, degrees, duration, easing, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    _monoBehaviour.StartCoroutine(RotateUi(_textComponent[occurrence].rectTransform, target, occurrence, degrees, duration, easing, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    _monoBehaviour.StartCoroutine(RotateUi(_imageComponent[occurrence].rectTransform, target, occurrence, degrees, duration, easing, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    SaveRotation(target, occurrence);

                    _monoBehaviour.StartCoroutine(RotateUi((RectTransform)_buttonComponent[occurrence].transform, target, occurrence, degrees, duration, easing, delay));
                    break;
            }
        }

        public void RotationForever(AnimationTarget target, int occurrence, float dps, float delay)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!IndexNullChecksPass(AnimationTarget.Panel, 0)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUiForever(_panelTransform, dps, delay));
                    break;
                case AnimationTarget.Text:
                    if (!IndexNullChecksPass(AnimationTarget.Text, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUiForever(_textComponent[occurrence].rectTransform, dps, delay));
                    break;
                case AnimationTarget.Image:
                    if (!IndexNullChecksPass(AnimationTarget.Image, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUiForever(_imageComponent[occurrence].rectTransform, dps, delay));
                    break;
                case AnimationTarget.Button:
                    if (!IndexNullChecksPass(AnimationTarget.Button, occurrence)) { return; }

                    _monoBehaviour.StartCoroutine(RotateUiForever((RectTransform)_buttonComponent[occurrence].transform, dps, delay));
                    break;
            }
        }

        // ----------------------------------------------------- ROTATE ANIMATION -----------------------------------------------------

        private IEnumerator RotateUi(RectTransform component, AnimationTarget target, int occurrence, float degrees, float duration, EasingType easing, float delay)
        {
            GetStartRotation(target, occurrence, out Quaternion startRotation); 
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

        private IEnumerator RotateUiForever(RectTransform component, float dps, float delay)
        {
            if (delay > 0) { yield return new WaitForSeconds(delay); }

            FlowKitEvents.InvokeRotateStart();

            float angle = 0f;
            while (true)
            {
                angle += dps * Time.deltaTime;
                component.localRotation = Quaternion.Euler(0, 0, angle);
                yield return null;
            }
        }

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private void GetStartRotation(AnimationTarget target, int occurrence, out Quaternion startRotation)
        {
            startRotation = Quaternion.identity;

            switch (target)
            {
                case AnimationTarget.Panel:
                    startRotation = _panelTransform.localRotation;
                    break;
                case AnimationTarget.Text:
                    startRotation = _textComponent[occurrence].rectTransform.localRotation;
                    break;
                case AnimationTarget.Image:
                    startRotation = _imageComponent[occurrence].rectTransform.localRotation;
                    break;
                case AnimationTarget.Button:
                    startRotation = _buttonComponent[occurrence].transform.localRotation;
                    break;
            }
        }

        private void SaveRotation(AnimationTarget target, int occurrence)
        {
            switch (target)
            {
                case AnimationTarget.Panel:
                    if (!_storedRotation[FlowKitConstants.PanelIndex][0])
                    {
                        _originalRotation[FlowKitConstants.PanelIndex][0] = _panelTransform.localRotation;
                        _storedRotation[FlowKitConstants.PanelIndex][0] = true;
                    }
                    break;
                case AnimationTarget.Text:
                    if (!_storedRotation[FlowKitConstants.TextIndex][occurrence])
                    {
                        _originalRotation[FlowKitConstants.TextIndex][occurrence] = _textComponent[occurrence].rectTransform.localRotation;
                        _storedRotation[FlowKitConstants.TextIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Image:
                    if (!_storedRotation[FlowKitConstants.ImageIndex][occurrence])
                    {
                        _originalRotation[FlowKitConstants.ImageIndex][occurrence] = _imageComponent[occurrence].rectTransform.localRotation;
                        _storedRotation[FlowKitConstants.ImageIndex][occurrence] = true;
                    }
                    break;
                case AnimationTarget.Button:
                    if (!_storedRotation[FlowKitConstants.ButtonIndex][occurrence])
                    {
                        _originalRotation[FlowKitConstants.ButtonIndex][occurrence] = ((RectTransform)_buttonComponent[occurrence].transform).localRotation;
                        _storedRotation[FlowKitConstants.ButtonIndex][occurrence] = true;
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
