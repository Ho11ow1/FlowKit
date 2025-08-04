using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlowKit.Sandbox
{
    public class SandboxDateCycle : MonoBehaviour
    {
        public static SandboxDateCycle Instance { get; private set; }

        [Header("Date variables")]
        [SerializeField] private DayOfWeek day = DayOfWeek.Monday;
        [SerializeField] private int week = 0;
        /// <summary>
        /// Returns the current day of the week.
        /// </summary>
        public DayOfWeek Day => day;
        /// <summary>
        /// Returns the index of the current day of the week.
        /// </summary>
        public int DayIndex => (int)day;
        /// <summary>
        /// Returns the current week number.
        /// </summary>
        public int Week => week;

        public enum DayOfWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        public static event UnityAction<DayOfWeek> OnDayChange;
        public static event UnityAction<int> OnWeekChange;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ----------------------------------------------------- GENERAL-PURPOSE INCREMENT METHODS -----------------------------------------------------

        /// <summary>
        /// Advances the day by one.
        /// <list type="bullet">
        ///   <item>
        ///     <description>If the day changes from Sunday to Monday a OnWeekChange will occur</description>
        ///   </item>
        /// </list>
        /// </summary>
        public void AdvanceDay()
        {
            if (day == DayOfWeek.Sunday)
            {
                day = DayOfWeek.Monday;
                week += 1;
                OnWeekChange?.Invoke(week);
            }
            else
            {
                day += 1;
            }

            OnDayChange?.Invoke(day);
        }

        /// <summary>
        /// Advances the day by a specified number of days.
        /// <list type="bullet">
        ///   <item>
        ///     <description>If the day changes from Sunday to Monday a OnWeekChange will occur</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="days">Specifies the amount of days to advance by</param>
        public void AdvanceDays(int days)
        {
            if (days < 0)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Days cannot be negative. Please provide a valid number of days to advance.");
                #endif
                return;
            }

            for (int i = 0; i < days; i++)
            {
                AdvanceDay();
            }
        }

        /// <summary>
        /// Advances the week by one.
        /// </summary>
        public void AdvanceWeek()
        {
            week += 1;
            OnWeekChange?.Invoke(week);
        }

        /// <summary>
        /// Advances the week by a specified number of weeks.
        /// </summary>
        /// <param name="weeks">Specifies the amount of weeks to advance by</param>
        public void AdvanceWeeks(int weeks)
        {
            if (weeks < 0)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Weeks cannot be negative. Please provide a valid number of weeks to advance.");
                #endif
                return;
            }

            for (int i = 0; i < weeks; i++)
            {
                AdvanceWeek();
            }
        }


        // ----------------------------------------------------- GENERAL-PURPOSE SETTERS -----------------------------------------------------

        /// <summary>
        /// Sets the day of the week based on the provided index.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does not change the week counter.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="index">Specifies the day index to set the current day to</param>
        public void SetDayByIndex(int index)
        {
            if (index < 0 || index > 6)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Index out of range. Please use a value between 0 and 6.");
                #endif
                return;
            }

            DayOfWeek newDay = (DayOfWeek)index;
            SetDay(newDay);
        }

        /// <summary>
        /// Sets the day of the week based on the provided DayOfWeek enum.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Does not change the week counter.</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <param name="day">Specifies the day to be set</param>
        public void SetDay(DayOfWeek day)
        {
            this.day = day;
            OnDayChange?.Invoke(day);
        }

        /// <summary>
        /// Sets the week number.
        /// </summary>
        /// <param name="week">Specifies the week to be set</param>
        public void SetWeek(int week)
        {
            if (week < 0)
            {
                #if UNITY_EDITOR
                Debug.LogWarning("Week cannot be negative. Please provide a valid week number.");
                #endif
                return;
            }

            this.week = week;
            OnWeekChange?.Invoke(week);
        }
    }
}
