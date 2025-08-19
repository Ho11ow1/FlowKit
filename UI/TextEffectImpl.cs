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
* FlowKit - TextEffects Animation Component
* Created by Hollow1
* 
* Applies visual effects to a TextMeshProUGUI component
* 
* Version: 1.2.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections;
using UnityEngine;
using TMPro;

using FlowKit.Common;
using System.Collections.Generic;

namespace FlowKit.UI
{
    [AddComponentMenu("")]
    internal class TextEffectImpl
    {
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly Utils.StringAutoIncreaseList _targetString = new Utils.StringAutoIncreaseList();
        private int _length;

        public TextEffectImpl(TextMeshProUGUI[] tmp, MonoBehaviour runner)
        {
            _textComponent = tmp;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void DelayTypeWrite(int occurrence, float delay = FlowKitConstants.TypeWriter.PerCharacterDelay)
        {
            if (!IndexNullChecksPass(occurrence)) { return; }

            _targetString[occurrence] = _textComponent[occurrence].text;
            _length = _targetString[occurrence].Length;
            _monoBehaviour.StartCoroutine(DelayWriter(occurrence, delay));
        }

        public void DurationTypeWrite(int occurrence, float duration = FlowKitConstants.TypeWriter.CompleteTextDuration)
        {
            if (!IndexNullChecksPass(occurrence)) { return; }

            _targetString[occurrence] = _textComponent[occurrence].text;
            _length = _targetString[occurrence].Length;
            _monoBehaviour.StartCoroutine(DurationWriter(occurrence, duration));
        }

        public void ColorCycleTwo(int occurrence, float duration, float delay, Color32 newColor)
        {
            if (!IndexNullChecksPass(occurrence)) { return; }

            Color32 oldColor = (Color32)(_textComponent[occurrence].color);
            _monoBehaviour.StartCoroutine(ColorCyclerTwo(occurrence, duration, delay, oldColor, newColor));
        }

        public void ColorCycleMulti(int occurrence, float duration, float delay, Color32[] colors)
        {
            if (!IndexNullChecksPass(occurrence)) { return; }

            Color32 originalColor = (Color32)(_textComponent[occurrence].color);
            _monoBehaviour.StartCoroutine(ColorCyclerMulti(occurrence, duration, delay, originalColor, colors));
        }


        // ----------------------------------------------------- TYPEWRITER EFFECT -----------------------------------------------------

        private IEnumerator DurationWriter(int occurrence, float duration)
        {
            FlowKitEvents.InvokeTypeWriteStart();
            _textComponent[occurrence].text = "";
            string currentText = "";

            float delay = 0f;
            if (duration > 0 && _length > 0) { delay = duration / _length; }

            foreach (char c in _targetString[occurrence])
            {
                currentText += c;
                _textComponent[occurrence].text = currentText;
                yield return new WaitForSeconds(delay);
            }

            if (_textComponent[occurrence].text != _targetString[occurrence]) { _textComponent[occurrence].text = _targetString[occurrence]; }
            FlowKitEvents.InvokeTypeWriteEnd();
        }

        private IEnumerator DelayWriter(int occurrence, float delay)
        {
            FlowKitEvents.InvokeTypeWriteStart();
            _textComponent[occurrence].text = "";
            string currentText = "";

            foreach (char c in _targetString[occurrence])
            {
                currentText += c;
                _textComponent[occurrence].text = currentText;
                yield return new WaitForSeconds(delay);
            }

            if (_textComponent[occurrence].text != _targetString[occurrence]) { _textComponent[occurrence].text = _targetString[occurrence]; }
            FlowKitEvents.InvokeTypeWriteEnd();
        }

        private IEnumerator ColorCyclerTwo(int occurrence, float duration, float delay, Color32 oldColor, Color32 newColor)
        {
            FlowKitEvents.InvokeColorCycleStart();

            float elapsedTime = 0f;
            bool infinite = duration == 0f;

            while (infinite || elapsedTime < duration)
            {
                var currentColor = _textComponent[occurrence].color;
                _textComponent[occurrence].color = currentColor == oldColor ? newColor : oldColor;

                yield return new WaitForSeconds(delay);
                elapsedTime += delay;
            }

            _textComponent[occurrence].color = oldColor;
            FlowKitEvents.InvokeColorCycleEnd();
        }

        private IEnumerator ColorCyclerMulti(int occurrence, float duration, float delay, Color32 originalColor, Color32[] colors)
        {
            if (colors == null || colors.Length == 0) { yield break; }

            FlowKitEvents.InvokeColorCycleStart();

            float elapsedTime = 0f;
            bool infinite = duration == 0f;
            int colorIndex = 0;

            while (infinite || elapsedTime < duration)
            {
                _textComponent[occurrence].color = colors[colorIndex];
                colorIndex = (colorIndex + 1) % colors.Length;

                yield return new WaitForSeconds(delay);
                elapsedTime += delay;
            }

            _textComponent[occurrence].color = originalColor;
            FlowKitEvents.InvokeColorCycleEnd();
        }

        // ----------------------------------------------------- PRIVATE UTILITIES -----------------------------------------------------

        private bool IndexNullChecksPass(int occurrence)
        {
            return occurrence < _textComponent.Length && _textComponent[occurrence] != null;
        }
    }
}
