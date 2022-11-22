using UnityEngine;
public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public delegate void TimerEvents();
    public static event TimerEvents TimerExpired;

    private void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                TimerExpired.Invoke();
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void Pause()
    {
        timerIsRunning = false;
    }

    public void Resume()
    {
        timerIsRunning = true;
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
    }

    public void SubtractTime(float amount)
    {
        timeRemaining -= amount;
    }
}
