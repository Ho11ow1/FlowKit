using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private bool isRunning = false;

    [SerializeField] private float time = 0f;
    [SerializeField, Range(0f, 3600f)] private float maxTime = 300f;
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
        SetCurrentTime(time);

        if (timerMode == TimerMode.StopWatch)
        {
            time += Time.deltaTime;

            if (maxTime > 0f && time >= maxTime)
            {
                time = maxTime;
                SetCurrentTime(time);
                StopTimer();
                TimerMaxReached?.Invoke();
            }
        }
        else if (timerMode == TimerMode.CountDown)
        {
            time -= Time.deltaTime;

            if (time <= 0f)
            {
                time = 0f;
                SetCurrentTime(time);
                StopTimer();
                TimerEnd?.Invoke();
            }
        }
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

    public void StartTimer()
    {
        if (timerMode == TimerMode.CountDown && maxTime > 0f)
        {
            time = maxTime;
        }

        isRunning = true;
        TimerStart?.Invoke();
    }

    public void StopTimer()
    {
        isRunning = false;

        ResetTimer();

        TimerEnd?.Invoke();
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
        }
    }

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

    public float GetTime()
    {
        return time;
    }

    public void SetTime(float time)
    {
        this.time = Mathf.Max(0f, time);
        SetCurrentTime(this.time);
    }

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

    public float GetMaxTime()
    {
        return maxTime;
    }

    public void SetMaxTime(float maxTime)
    {
        this.maxTime = Mathf.Max(0f, maxTime);
    }

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

    public bool MaxTimeExists()
    {
        return maxTime > 0f;
    }

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
