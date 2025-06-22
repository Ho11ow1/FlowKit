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
* FlowKit - TypeWriter Animation Component
* Created by Hollow1
* 
* Applies a typewriter effect to a TextMeshProUGUI component
* 
* Version: 1.0.0
* GitHub: https://github.com/Ho11ow1/FlowKit
* License: Apache License 2.0
* -------------------------------------------------------- */
using System.Collections;
using UnityEngine;
using TMPro;

using FlowKit.Common;

namespace FlowKit.UI
{
    [AddComponentMenu("")]
    internal class TypeWrite
    {
        private readonly TextMeshProUGUI[] _textComponent;
        private readonly MonoBehaviour _monoBehaviour;

        private readonly Utils.StringAutoIncreaseList _targetString = new Utils.StringAutoIncreaseList();
        private int _length;

        private const float _standardDelay = 0.3f;
        private const float _standardDuration = 3f;

        public TypeWrite(TextMeshProUGUI[] tmp, MonoBehaviour runner)
        {
            _textComponent = tmp;
            _monoBehaviour = runner;
        }

        // ----------------------------------------------------- PUBLIC API -----------------------------------------------------

        public void TypeWriterDelay(int occurrence, float delay = _standardDelay)
        {
            _targetString[occurrence] = _textComponent[occurrence].text;
            _length = _targetString[occurrence].Length;
            _monoBehaviour.StartCoroutine(WriterDelay(occurrence, delay));
        }

        public void TypeWriterDuration(int occurrence, float duration = _standardDuration)
        {
            _targetString[occurrence] = _textComponent[occurrence].text;
            _length = _targetString[occurrence].Length;
            _monoBehaviour.StartCoroutine(WriterDuration(occurrence, duration));
        }

        // ----------------------------------------------------- TYPEWRITER EFFECT -----------------------------------------------------

        private IEnumerator WriterDuration(int occurrence, float duration)
        {
            if (_textComponent == null) { yield break; }

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

        private IEnumerator WriterDelay(int occurrence, float delay)
        {
            if (_textComponent == null) { yield break; }

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
    }
}