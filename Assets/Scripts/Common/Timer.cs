using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject TimerCollector;

    public UnityAction OnTimeRanOut;
    public UnityAction OnTick;

    public bool IsLoop;

    public float MaxTime => maxTime;
    public float CurrentTime => currentTime;
    public bool IsPause => isPause;
    public bool IsComplited => currentTime <= 0;

    private float maxTime;
    private float currentTime;
    private bool isPause;

    private void Update()
    {
        if(isPause) return;

        currentTime -= Time.deltaTime;

        OnTick?.Invoke();

        if (currentTime <= 0)
        {
            currentTime = 0;

            OnTimeRanOut?.Invoke();

            if (IsLoop)
                currentTime = maxTime;
        }            
    }

    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;
        timer.IsLoop = isLoop;

        return timer;
    }

    public static Timer CreateTimer(float time)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;

        return timer;
    }

    public void Play()
    { 
        isPause = false;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Complited()
    { 
        isPause = false;
        currentTime = 0;
    }

    public void StopWithoutEvent()
    {
        Destroy(this);
    }

    public void Restart(float time)
    {
        maxTime = time;
    }

    public void Restart()
    {
        currentTime = maxTime; 
    }
}

