using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace FlowKit
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI timerText;
        private bool isRunning = false;

        [SerializeField] private float time = 0f;
        [SerializeField, Range(0f, 3600f)] private float maxTime = 300f;
        [Tooltip("If Countdown is selected, timer will ignore the set Time and start from Max Timer")]
        [SerializeField] TimerMode timerMode = TimerMode.StopWatch;

        private enum TimerMode
        {
            StopWatch,
            CountDown
        }

        public static event UnityAction TimerStart;
        public static event UnityAction TimerEnd;
        public static event UnityAction TimerMaxReached;

        void Awake()
        {
            timerText = GetComponent<TextMeshProUGUI>();
            if (timerText == null)
            {
                Debug.LogError("Timer component requires a TextMeshProUGUI component.");
            }
            StartTimer();
        }

        void Update()
        {
            if (!isRunning || timerText == null) { return; }

            UpdateTimerState();
        }

        private void UpdateTimerState()
        {
            if (timerMode == TimerMode.StopWatch)
            {
                time += Time.deltaTime;

                if (maxTime > 0f && time >= maxTime)
                {
                    time = maxTime;
                    StopTimer();
                }
            }
            else if (timerMode == TimerMode.CountDown)
            {
                time -= Time.deltaTime;

                if (time <= 0f)
                {
                    time = 0f;
                    StopTimer();
                }
            }

            SetCurrentTime(time);
        }

        private void SetCurrentTime(float setTime)
        {
            var hours = Mathf.FloorToInt(setTime / 3600);
            var minutes = Mathf.FloorToInt((setTime % 3600) / 60);
            var seconds = Mathf.FloorToInt(setTime % 60);

            if (hours > 0)
            {
                timerText.text = $"{hours}:{minutes:D2}:{seconds:D2}";
            }
            else
            {
                timerText.text = $"{minutes}:{seconds:D2}";
            }
        }

        // ----------------------------------------------------- PUBLIC CONDITION METHODS -----------------------------------------------------

        /// <summary>
        /// Start the timer if it is not already running.
        /// </summary>
        public void StartTimer()
        {
            if (timerMode == TimerMode.CountDown && maxTime > 0f)
            {
                time = maxTime;
            }

            isRunning = true;
            TimerStart?.Invoke();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            isRunning = false;

            if (maxTime > 0f && time >= maxTime)
            {
                TimerMaxReached?.Invoke();
            }
            TimerEnd?.Invoke();
        }

        /// <summary>
        /// Pauses the timer if it is running.
        /// </summary>
        public void PauseTimer()
        {
            if (isRunning)
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// Resumes the timer if it is paused.
        /// </summary>
        public void ResumeTimer()
        {
            if (!isRunning)
            {
                isRunning = true;
            }
        }

        /// <summary>
        /// Resets the timer to its initial state.
        /// </summary>
        public void ResetTimer()
        {
            if (timerMode == TimerMode.CountDown && maxTime > 0f)
            {
                time = maxTime;
            }
            else
            {
                time = 0f;
            }
            SetCurrentTime(time);
        }

        // ----------------------------------------------------- PUBLIC TIME METHODS -----------------------------------------------------

        /// <summary>
        /// Returns the current time in seconds as a float.
        /// </summary>
        public float GetTime()
        {
            return time;
        }

        /// <summary>
        /// Returns the current time in seconds as an integer.
        /// </summary>
        public int GetTimeInSeconds()
        {
            return Mathf.FloorToInt(time);
        }

        /// <summary>
        /// Sets the curremt timer value.
        /// </summary>
        /// <param name="time">Specifies the time in seconds</param>
        public void SetTime(float time)
        {
            this.time = Mathf.Max(0f, time);
            SetCurrentTime(this.time);
        }

        /// <summary>
        /// Returns the current time in a HH:MM:SS format if hours are greater than 0, otherwise in MM:SS format.
        /// </summary>
        public string GetFormattedTime()
        {
            var hours = Mathf.FloorToInt(time / 3600);
            var minutes = Mathf.FloorToInt((time % 3600) / 60);
            var seconds = Mathf.FloorToInt(time % 60);

            if (hours > 0)
            {
                return $"{hours}:{minutes:D2}:{seconds:D2}";
            }
            else
            {
                return $"{minutes}:{seconds:D2}";
            }
        }

        // ----------------------------------------------------- PUBLIC MAX TIME METHODS -----------------------------------------------------

        /// <summary>
        /// Returns the maximum time in seconds as a float.
        /// </summary>
        public float GetMaxTime()
        {
            return maxTime;
        }

        /// <summary>
        /// Return the maximum time in seconds as an integer.
        /// </summary>
        public int GetMaxTimeInSeconds()
        {
            return Mathf.FloorToInt(maxTime);
        }

        /// <summary>
        /// Sets the maximum time for the timer.
        /// </summary>
        /// <param name="maxTime">Specifies the maximum time in seconds</param>
        public void SetMaxTime(float maxTime)
        {
            this.maxTime = Mathf.Max(0f, maxTime);
        }

        /// <summary>
        /// Returns the remaining time in seconds based on the timer mode.
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>CountDown:</b>: Returns the current time</description>
        ///     </item>
        ///     <item>
        ///         <description><b>StopWatch:</b>: Returns maxTime - currentTime, or float.MaxValue if maxTime is less than or equal to 0</description>
        ///     </item>
        /// </list>
        /// </summary>
        public float GetRemainingTime()
        {
            if (timerMode == TimerMode.CountDown)
            {
                return Mathf.Max(0f, time);
            }
            else
            {
                if (maxTime <= 0f) { return float.MaxValue; }

                return Mathf.Max(0f, maxTime - time);
            }
        }

        /// <summary>
        /// Returns true if the timer has a maximum time set above 0.
        /// </summary>
        public bool MaxTimeExists()
        {
            return maxTime > 0f;
        }

        /// <summary>
        /// Returns the timer progress as a percentage (0-100).
        /// <list type="bullet">
        ///     <item>
        ///         <description><b>CountDown:</b>: Progress from 0% (full time remaining) to 100% (timer finished)</description>
        ///     </item>
        ///     <item>
        ///         <description><b>StopWatch:</b>: Progress from 0% (just started) to 100% (maxTime reached)</description>
        ///     </item>
        /// </list>
        /// Returns 0% if no maxTime is set.
        /// </summary>
        public float GetProgressPercentage()
        {
            if (timerMode == TimerMode.CountDown)
            {
                if (maxTime <= 0f) { return 0f; }

                return Mathf.Clamp01((maxTime - time) / maxTime) * 100f;
            }
            else
            {
                if (maxTime <= 0f) { return 0f; }

                return Mathf.Clamp01(time / maxTime) * 100f;
            }
        }
    }
}
