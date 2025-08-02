using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace FlowKit.Rpg
{
    public class RpgClock : MonoBehaviour
    {
        public static RpgClock Instance { get; private set; }

        private bool isVisual;
        private TextMeshProUGUI timeText;
        private TimeOfDay currentTimeOfDay;
        /// <summary>
        /// Returns the current Hour.
        /// </summary>
        public int Hour => totalMinutes / 60;
        /// <summary>
        /// Returns the current minute of the hour.
        /// </summary>
        public int Minute => totalMinutes % 60;

        [Header("Time related variables")]
        [SerializeField] private int totalMinutes = 0;
        [SerializeField] private TimeFormat timeFormat = TimeFormat.Military;
        [SerializeField] private bool trackTime = false;

        public enum TimeOfDay
        {
            Morning,
            Afternoon,
            Evening,
            Night,
            Midnight
        }

        public enum TimeFormat
        {
            Military,
            Meridian
        }

        public static event UnityAction<TimeOfDay> TimeOfDayTrigger;

        void Awake()
        {
            if (trackTime)
            {
                currentTimeOfDay = GetTimeOfDayByHour(Hour);
            }

            if (Instance == null)
            {
                Instance = this;

                if (gameObject.GetComponent<RectTransform>())
                {
                    isVisual = true;
                    if (TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
                    {
                        timeText = text;
                        SetFormattedTime();
                    }
                    #if UNITY_EDITOR
                    else
                    {
                        Debug.LogWarning("RpgClock requires a TextMeshProUGUI component to display time.");
                    }
                    #endif
                }
                if (!isVisual)
                {
                    DontDestroyOnLoad(gameObject);
                }
                return;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /*
         * Comment out Update() if you do not wish to display any time information in any way.
         */
        void Update()
        {
            if (timeText != null && trackTime)
            {
                timeText.text = SetFormattedTime();
            }
            else if (timeText != null && !trackTime)
            {
                timeText.text = currentTimeOfDay.ToString();
            }
        }


        private string SetFormattedTime()
        {
            switch (timeFormat)
            {
                case TimeFormat.Military:
                    return $"{Hour:D2}:{Minute:D2}";
                case TimeFormat.Meridian:
                    string period = Hour >= 12 ? "PM" : "AM";
                    int displayHours = Hour == 0 ? 12 : (Hour > 12 ? Hour - 12 : Hour);

                    return $"{displayHours:D2}:{Minute:D2}{period.ToUpper()}";
                default:
                    return "00:00";
            }
        }

        // ----------------------------------------------------- GENERAL-PURPOSE INCREMENT METHODS -----------------------------------------------------

        /// <summary>
        /// Advances the time by a specified number of minutes.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does nothing if time tracking is disabled.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="minutes">Specifies the amount of minutes to pass</param>
        public void AdvanceTime(int minutes)
        {
            if (!trackTime)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("RpgClock is not tracking time.\nPlease enable time tracking to use this method.");
                #endif
                return;
            }

            totalMinutes = (totalMinutes + minutes) % 1440;

            var newTimeOfDay = GetTimeOfDayByHour(Hour);
            if (newTimeOfDay != currentTimeOfDay)
            {
                currentTimeOfDay = newTimeOfDay;
                TimeOfDayTrigger?.Invoke(currentTimeOfDay);
            }
        }

        /// <summary>
        /// Advances the current TimeOfDay by one step.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Additionaly modifies the time if time tracking is enabled</description>
        ///   </item>
        /// </list>
        /// </summary>
        public void AdvanceTimeOfDay()
        {
            currentTimeOfDay = (TimeOfDay)(((int)currentTimeOfDay + 1) % 5);

            if (trackTime)
            {
                switch (currentTimeOfDay)
                {
                    case TimeOfDay.Morning:
                        totalMinutes = 6 * 60;
                        break;
                    case TimeOfDay.Afternoon:
                        totalMinutes = 12 * 60;
                        break;
                    case TimeOfDay.Evening:
                        totalMinutes = 18 * 60;
                        break;
                    case TimeOfDay.Night:
                        totalMinutes = 23 * 60;
                        break;
                    case TimeOfDay.Midnight:
                        totalMinutes = 2 * 60;
                        break;
                }
            }

            TimeOfDayTrigger?.Invoke(currentTimeOfDay);
        }

        // ----------------------------------------------------- MULTI-STYLE RPG GETTERS -----------------------------------------------------


        /// <summary>
        /// Return the current TimeOfDay.
        /// </summary>
        public TimeOfDay GetCurrentTimeOfDay()
        {
            return currentTimeOfDay;
        }

        /// <summary>
        /// Returns the current TimeOfDay based on the tracked hour.
        /// <list type="bullet">
        ///   <item>
        ///     <description>If trackTime is false, it will return the currentTimeOfDay without checking the hour.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="hour">Specifies the tracked hour to base TimeOfDay off of | Range of 0 - 23</param>
        public TimeOfDay GetTimeOfDayByHour(int hour)
        {
            if (!trackTime)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("RpgClock is not tracking time.\nPlease enable hour tracking to use this method.");
                #endif
                return currentTimeOfDay;
            }
            var clampedHour = Mathf.Clamp(hour, 0, 23);

            if (clampedHour >= 2 && clampedHour <= 5)
            {
                return TimeOfDay.Midnight;
            }
            else if (clampedHour >= 6 && clampedHour <= 11)
            {
                return TimeOfDay.Morning;
            }
            else if (clampedHour >= 12 && clampedHour <= 17)
            {
                return TimeOfDay.Afternoon;
            }
            else if (clampedHour >= 18 && clampedHour <= 22)
            {
                return TimeOfDay.Evening;
            }
            else
            {
                return TimeOfDay.Night;
            }
        }

        // ----------------------------------------------------- MULTI-STYLE RPG SETTERS -----------------------------------------------------

        /// <summary>
        /// Sets the TimeOfDay based on the current hour.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does nothing if time tracking is false.</description>
        ///   </item>
        ///   <item>
        ///     <description>Does nothing if the TimeOfDay does not change.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="hour">Specifies the hour to set the time to | Range of 0 - 23</param>
        public void SetTimeOfDayByHour(int hour)
        {
            if (!trackTime)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("RpgClock is not tracking time.\nPlease enable hour tracking to use this method.");
                #endif
                return;
            }

            var clampedHour = Mathf.Clamp(hour, 0, 23);

            var timeOfDay = GetTimeOfDayByHour(clampedHour);
            if (timeOfDay != currentTimeOfDay)
            {
                currentTimeOfDay = timeOfDay;
                totalMinutes = clampedHour * 60;
                TimeOfDayTrigger?.Invoke(currentTimeOfDay);
            }
        }

        /// <summary>
        /// Sets the TimeOfDay directly.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does nothing if the TimeOfDay does not change.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="timeOfDay">Specifies the TimeOfDay to be set</param>
        public void SetTimeOfDay(TimeOfDay timeOfDay)
        {
            if (timeOfDay == currentTimeOfDay) { return; }

            currentTimeOfDay = timeOfDay;
            TimeOfDayTrigger?.Invoke(currentTimeOfDay);
        }
    }
}
