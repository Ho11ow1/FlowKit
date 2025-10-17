using System;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        public enum TimeOfDay
        {
            Morning,
            Noon,
            Afternoon,
            Evening,
            Night,
            Midnight
        }

        private static readonly string[] _daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        private static readonly string[] _weekendDays = { "Saturday", "Sunday" };
        private TimeOfDay dayPeriod = TimeOfDay.Morning;
        private string day = _daysOfWeek[0];
        private int dayCount = 1;
        private int week = 1;
        private bool isWeekend = false;

        /// <summary>
        /// Returns the current day period.
        /// </summary>
        public TimeOfDay DayPeriod => dayPeriod;
        /// <summary>
        /// Returns the current day of the week.
        /// </summary>
        public string Day => day;
        /// <summary>
        /// Returns the amount of days passed since starting.
        /// </summary>
        public int DayCount => dayCount;
        /// <summary>
        /// Returns the current week number.
        /// </summary>
        public int Week => week;
        /// <summary>
        /// Returns true if the current day is a weekend day.
        /// </summary>
        public bool IsWeekend => isWeekend;

        public static event UnityAction<TimeOfDay> OnDayPeriodChange;
        public static event UnityAction<string> OnDayChange;
        public static event UnityAction<int> OnWeekChange;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void CorrectWeekendState()
        {
            isWeekend = Array.Exists(_weekendDays, day => day == this.day);
        }

        /// <summary>
        /// Advances the current day period to the next one.
        /// </summary>
        public void AdvanceDayPeriod()
        {
            switch (dayPeriod)
            {
                case TimeOfDay.Morning:
                    dayPeriod = TimeOfDay.Noon;
                    break;
                case TimeOfDay.Noon:
                    dayPeriod = TimeOfDay.Afternoon;
                    break;
                case TimeOfDay.Afternoon:
                    dayPeriod = TimeOfDay.Evening;
                    break;
                case TimeOfDay.Evening:
                    dayPeriod = TimeOfDay.Night;
                    break;
                case TimeOfDay.Night:
                    dayPeriod = TimeOfDay.Midnight;
                    break;
                case TimeOfDay.Midnight:
                    dayPeriod = TimeOfDay.Morning;
                    break;
            }

            OnDayPeriodChange?.Invoke(dayPeriod);

            if (dayPeriod == TimeOfDay.Morning)
            {
                AdvanceDay();
            }
        }

        /// <summary>
        /// Sets the current day period.
        /// </summary>
        /// <param name="timeOfDay">TimeOfDay enum value to set the dayPeriod to</param>
        public void SetDayPeriod(TimeOfDay timeOfDay)
        {
            dayPeriod = timeOfDay;
        }

        /// <summary>
        /// Advances the current day by 1.
        /// </summary>
        public void AdvanceDay()
        {
            var previousDayIndex = Array.IndexOf(_daysOfWeek, day);

            if (previousDayIndex == _daysOfWeek.Length - 1)
            {
                day = _daysOfWeek[0];
                AdvanceWeek();
            }
            else
            {
                day = _daysOfWeek[previousDayIndex + 1];
            }

            dayCount += 1;
            CorrectWeekendState();

            OnDayChange?.Invoke(day);
        }

        /// <summary>
        /// Sets the current day of week via its name.
        /// </summary>
        /// <param name="dayName">Name of the day of the week</param>
        /// <exception cref="ArgumentException">Thrown when the dayName is not a valid day</exception>
        public void SetDay(string dayName)
        {
            if (!Array.Exists(_daysOfWeek, day => day == this.day))
            {
                throw new ArgumentException(nameof(day), $"Day '{day}' is not a valid day of the week");
            }

            day = dayName;
            CorrectWeekendState();
        }

        /// <summary>
        /// Sets the current day via index, where 0 = Monday and 6 = Sunday.
        /// </summary>
        /// <param name="dayIndex">Index between 0 and 6 for the daysOfWeek array</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the dayIndex is outside the bounds of the daysOfWeek array</exception>
        public void SetDay(int dayIndex)
        {
            if (dayIndex < 0 || dayIndex >= _daysOfWeek.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(dayIndex), $"Day index '{dayIndex}' is outside the bound of the daysOfWeek array");
            }

            day = _daysOfWeek[dayIndex];
            CorrectWeekendState();
        }

        /// <summary>
        /// Advances the current week by 1.
        /// </summary>
        public void AdvanceWeek()
        {
            week += 1;

            OnWeekChange?.Invoke(week);
        }

        /// <summary>
        /// Sets the current week number.
        /// </summary>
        /// <param name="weekNumber">Week number to set the week to</param>
        public void SetWeek(int weekNumber)
        {
            week = weekNumber;
        }
    }
}
