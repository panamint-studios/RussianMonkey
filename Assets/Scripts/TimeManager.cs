using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //How much time will it take before enemies arrive
    [Range(0, 10)]
    public int startingMinRemaining = 3;
    [Range(0, 59)]
    public int startingSecRemaining = 30;

    private float startingTimeToSeconds;

    bool countdownStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        startingTimeToSeconds = startingMinRemaining * 60 + startingSecRemaining;
        //StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownStarted)
        {
            Countdown();
        }
    }

    void Countdown()
    {
        startingTimeToSeconds -= Time.deltaTime;

        float minutes = Mathf.Floor(startingTimeToSeconds / 60);
        float seconds = Mathf.RoundToInt(startingTimeToSeconds % 60);

        Debug.Log(minutes + ":" + seconds);
    }

    public void StartCountdown()
    {
        countdownStarted = true;
    }

    public void AddSeconds(int secs)
    {
        startingTimeToSeconds += secs;
    }

    public void SubtractSeconds(int secs)
    {
        startingTimeToSeconds -= secs;
    }
}
