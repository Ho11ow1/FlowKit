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
        private DayPeriod currentDayPeriod;
        /// <summary>
        /// Returns the current DayPeriod.
        /// </summary>
        public DayPeriod CurrentDayPeriod => currentDayPeriod;
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

        public enum DayPeriod
        {
            Morning,
            Afternoon,
            Evening,
            Night,
            Midnight
        }

        private enum TimeFormat
        {
            Military,
            Meridian
        }

        public static event UnityAction<DayPeriod> OnDayPeriodChange;

        void Awake()
        {
            if (trackTime)
            {
                currentDayPeriod = GetDayPeriodByHour(Hour);
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
            if (!isVisual) { return; }

            if (timeText != null && trackTime)
            {
                timeText.text = SetFormattedTime();
            }
            else if (timeText != null && !trackTime)
            {
                timeText.text = currentDayPeriod.ToString();
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

            var newdayPeriod = GetDayPeriodByHour(Hour);
            if (newdayPeriod != currentDayPeriod)
            {
                currentDayPeriod = newdayPeriod;
                OnDayPeriodChange?.Invoke(currentDayPeriod);
            }
        }

        /// <summary>
        /// Advances the current DayPeriod by one step.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Additionaly modifies the time if time tracking is enabled</description>
        ///   </item>
        /// </list>
        /// </summary>
        public void AdvanceDayPeriod()
        {
            currentDayPeriod = (DayPeriod)(((int)currentDayPeriod + 1) % 5);

            if (trackTime)
            {
                switch (currentDayPeriod)
                {
                    case DayPeriod.Morning:
                        totalMinutes = 6 * 60;
                        break;
                    case DayPeriod.Afternoon:
                        totalMinutes = 12 * 60;
                        break;
                    case DayPeriod.Evening:
                        totalMinutes = 18 * 60;
                        break;
                    case DayPeriod.Night:
                        totalMinutes = 23 * 60;
                        break;
                    case DayPeriod.Midnight:
                        totalMinutes = 2 * 60;
                        break;
                }
            }

            OnDayPeriodChange?.Invoke(currentDayPeriod);
        }

        // ----------------------------------------------------- MULTI-STYLE RPG GETTERS -----------------------------------------------------

        /// <summary>
        /// Returns the current DayPeriod based on the tracked hour.
        /// <list type="bullet">
        ///   <item>
        ///     <description>If trackTime is false, it will return the currentDayPeriod without checking the hour.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="hour">Specifies the tracked hour to base DayPeriod off of | Range of 0 - 23</param>
        public DayPeriod GetDayPeriodByHour(int hour)
        {
            if (!trackTime)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("RpgClock is not tracking time.\nPlease enable hour tracking to use this method.");
                #endif
                return currentDayPeriod;
            }
            var clampedHour = Mathf.Clamp(hour, 0, 23);

            if (clampedHour >= 2 && clampedHour <= 5)
            {
                return DayPeriod.Midnight;
            }
            else if (clampedHour >= 6 && clampedHour <= 11)
            {
                return DayPeriod.Morning;
            }
            else if (clampedHour >= 12 && clampedHour <= 17)
            {
                return DayPeriod.Afternoon;
            }
            else if (clampedHour >= 18 && clampedHour <= 22)
            {
                return DayPeriod.Evening;
            }
            else
            {
                return DayPeriod.Night;
            }
        }

        // ----------------------------------------------------- MULTI-STYLE RPG SETTERS -----------------------------------------------------

        /// <summary>
        /// Sets the DayPeriod based on the current hour and updates the current hour.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does nothing if time tracking is false.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="hour">Specifies the hour to set the time to | Range of 0 - 23</param>
        public void SetDayPeriodByHour(int hour)
        {
            if (!trackTime)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("RpgClock is not tracking time.\nPlease enable hour tracking to use this method.");
                #endif
                return;
            }

            var clampedHour = Mathf.Clamp(hour, 0, 23);

            var dayPeriod = GetDayPeriodByHour(clampedHour);
            currentDayPeriod = dayPeriod;
            totalMinutes = clampedHour * 60;
            OnDayPeriodChange?.Invoke(currentDayPeriod);
        }

        /// <summary>
        /// Sets the DayPeriod directly.
        /// </summary>
        /// <param name="dayPeriod">Specifies the DayPeriod to be set</param>
        public void SetDayPeriod(DayPeriod dayPeriod)
        {
            currentDayPeriod = dayPeriod;
            OnDayPeriodChange?.Invoke(currentDayPeriod);
        }
    }
}
