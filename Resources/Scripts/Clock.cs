using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FlowKit
{
    public class Clock : MonoBehaviour
    {
        private TextMeshProUGUI clockText;

        void Awake()
        {
            clockText = GetComponent<TextMeshProUGUI>();
            if (clockText == null)
            {
                Debug.LogError("Clock component requires a TextMeshProUGUI component.");
            }
            StartClock();
        }

        private void Update()
        {
            if (clockText == null) { return; }

            UpdateClock();
        }

        
    }
}
