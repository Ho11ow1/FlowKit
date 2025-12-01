using System;
using UnityEngine.Events;

namespace FlowKit
{
    public static class TimeManager
    {
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
        private static TimeOfDay dayPeriod = TimeOfDay.Morning;
        private static string day = _daysOfWeek[0];
        private static int dayCount = 1;
        private static int week = 1;
        private static bool isWeekend = false;

        /// <summary>
        /// Returns the current day period.
        /// </summary>
        public static TimeOfDay DayPeriod => dayPeriod;
        /// <summary>
        /// Returns the current day of the week.
        /// </summary>
        public static string Day => day;
        /// <summary>
        /// Returns the amount of days passed since starting.
        /// </summary>
        public static int DayCount => dayCount;
        /// <summary>
        /// Returns the current week number.
        /// </summary>
        public static int Week => week;
        /// <summary>
        /// Returns true if the current day is a weekend day.
        /// </summary>
        public static bool IsWeekend => isWeekend;

        public static event UnityAction<TimeOfDay> OnDayPeriodChange;
        public static event UnityAction<string> OnDayChange;
        public static event UnityAction<int> OnWeekChange;

        private static void CorrectWeekendState()
        {
            isWeekend = Array.Exists(_weekendDays, d => d == day);
        }

        /// <summary>
        /// Advances the current day period to the next one.
        /// </summary>
        public static void AdvanceDayPeriod()
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
        public static void SetDayPeriod(TimeOfDay timeOfDay)
        {
            dayPeriod = timeOfDay;
        }

        /// <summary>
        /// Advances the current day by 1.
        /// </summary>
        public static void AdvanceDay()
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
        public static void SetDay(string dayName)
        {
            if (!Array.Exists(_daysOfWeek, d => d == day))
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
        public static void SetDay(int dayIndex)
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
        public static void AdvanceWeek()
        {
            week += 1;

            OnWeekChange?.Invoke(week);
        }

        /// <summary>
        /// Sets the current week number.
        /// </summary>
        /// <param name="weekNumber">Week number to set the week to</param>
        public static void SetWeek(int weekNumber)
        {
            week = weekNumber;
        }

        /// <summary>
        /// Collects and returns a TimeData class
        /// </summary>
        /// <returns>TimeData class holding all time related variables</returns>
        public static TimeData GetSaveData()
        {
            return new TimeData
            {
                dayPeriod = dayPeriod,
                day = day,
                dayCount = dayCount,
                week = week,
                isWeekend = isWeekend
            };
        }

        /// <summary>
        /// Loads and sets all data from TimeData
        /// </summary>
        /// <param name="data">The TimeData to load</param>
        public static void LoadSaveData(TimeData data)
        {
            dayPeriod = data.dayPeriod;
            day = data.day;
            dayCount = data.dayCount;
            week = data.week;
            isWeekend = data.isWeekend;
        }
    }

    [System.Serializable]
    public class TimeData
    {
        public TimeManager.TimeOfDay dayPeriod;
        public string day;
        public int dayCount;
        public int week;
        public bool isWeekend;
    }
}
