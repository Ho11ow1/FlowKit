using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using TMPro;

namespace FlowKit
{
    public class OnlineClock : MonoBehaviour
    {
        private TextMeshProUGUI clockText;
        private bool isRunning = false;
        private double unixTime = 0f;

        [Header("Clock Settings")]
        [SerializeField, Range(0f, 60f)] private float syncInterval = 60f;
        [SerializeField] ClockType clockType = ClockType.UseLocalTime;
        [SerializeField] ClockFormat clockFormat = ClockFormat.Military;
        [SerializeField] CycleEventType cycleEventType = CycleEventType.Daily;

        private enum ClockType
        {
            UseLocalTime,
            ServerSyncOnStart,
            KeepServerSync
        }

        private enum ClockFormat
        {
            Military,
            Meridian
        }

        private enum CycleEventType
        {
            Daily,
            Hourly,
            None
        }

        public static event UnityAction ClockCycled;

        void Awake()
        {
            clockText = GetComponent<TextMeshProUGUI>();
            if (clockText == null)
            {
                Debug.LogError("Clock component requires a TextMeshProUGUI component.");
            }
        }

        void Start()
        {
            if (clockType == ClockType.UseLocalTime)
            {
                unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                isRunning = true;
            }
            else if (clockType == ClockType.ServerSyncOnStart)
            {
                ServerSyncOnStart();
            }
            else if (clockType == ClockType.KeepServerSync)
            {
                ServerSync();
            }
        }

        void Update()
        {
            if (!isRunning || clockText == null) { return; }

            if (clockType != ClockType.KeepServerSync)
            {
                UpdateClock();
            }
        }

        private void ServerSyncOnStart()
        {
            RequestServerTime();
            isRunning = true;
        }

        private void ServerSync()
        {
            InvokeRepeating(nameof(RequestServerTime), 0f, syncInterval);
            isRunning = true;
        }

        private void SetTime(double setTime)
        {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds((long)setTime).DateTime;
            var hours = dateTime.Hour;
            var minutes = dateTime.Minute;

            if (clockFormat == ClockFormat.Military)
            {
                clockText.text = $"{hours}:{minutes:D2}";
            }
            else
            {
                string period = hours >= 12 ? "PM" : "AM";
                int displayHours = hours == 0 ? 12 : (hours > 12 ? hours - 12 : hours);

                clockText.text = $"{displayHours}:{minutes:D2} {period.ToUpper()}";
            }
        }

        // ----------------------------------------------------- MULTI UPDATE LOGIC -----------------------------------------------------

        private void UpdateClock()
        {
            double previousTime = unixTime;
            unixTime += Time.deltaTime;

            if (cycleEventType != CycleEventType.None)
            {
                var prevDateTime = DateTimeOffset.FromUnixTimeSeconds((long)previousTime);
                var currentDateTime = DateTimeOffset.FromUnixTimeSeconds((long)unixTime);

                if (cycleEventType == CycleEventType.Hourly)
                {
                    if (prevDateTime.Hour != currentDateTime.Hour)
                    {
                        ClockCycled?.Invoke();
                    }
                }
                else if (cycleEventType == CycleEventType.Daily)
                {
                    if (prevDateTime.Day != currentDateTime.Day)
                    {
                        ClockCycled?.Invoke();
                    }
                }
            }

            SetTime(unixTime);
        }

        // ----------------------------------------------------- SERVER REQUEST LOGIC -----------------------------------------------------

        private void RequestServerTime()
        {
            StartCoroutine(GetServerTime());
        }

        /*
         * Replace the UnityWebRequest URL with your server's endpoint that returns the current unixTime
         * 
         * Unity may have issues with SSL certificates, so ensure your server uses a valid certificate,
         * or set user settings to allow http requests.
         * 
         * Edit -> Project Settings -> Player -> Other Settings -> Configuration -> Allow downloads over HTTP: Always allowed
         */
        private IEnumerator GetServerTime()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(""))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        var json = JsonUtility.FromJson<WorldTimeResponse>(request.downloadHandler.text);
                        unixTime = json.unixtime;
                        SetTime(unixTime);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Failed to parse server time: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogError("Failed to get unixTime from server.");
                }
            }
        }

        [System.Serializable]
        private class WorldTimeResponse
        {
            public double unixtime;
        }
    }
}
