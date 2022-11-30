using UnityEngine;
public class Timer : MonoBehaviour
{
    public float MaxTime = 60;
    public float TimeRemaining;
    public bool TimerIsRunning = false;
    public delegate void TimerEvents();
    public static event TimerEvents TimerExpired;

    void Start()
    {
        TimeRemaining = MaxTime;
        TimerIsRunning = true;
    }

    void Update()
    {
        if (TimerIsRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else
            {
                if (TimerExpired != null) TimerExpired.Invoke();
                TimeRemaining = 0;
                TimerIsRunning = false;
            }
        }
    }

    public void Pause()
    {
        TimerIsRunning = false;
    }

    public void Resume()
    {
        TimerIsRunning = true;
    }

    public void AddTime(float amount)
    {
        TimeRemaining = Mathf.Min(TimeRemaining + amount, MaxTime);
    }

    public void SubtractTime(float amount)
    {
        TimeRemaining -= amount;
    }
}
