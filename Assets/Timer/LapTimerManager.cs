using UnityEngine;
using UnityEngine.UI;  
using TMPro;
using Application;

public class LapTimer : MonoBehaviour, IRaceTimer
{
    public TMP_Text timerText;

    private float startTime;
    public bool isTiming = false;
    public int lapCount = 0;
    public float lapTime;
    public float[] lapTimes;
    private int maxLaps = 5;
    public float currentTime;

    void Start()
    {
        lapTimes = new float[maxLaps];
        timerText.text = "00:00:00";
    }

    void Update()
    {
        if (isTiming)
        {
            currentTime = Time.time - startTime;
            timerText.text = GetFormattedTime();
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isTiming = true;
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime % 60F);
        int milliseconds = Mathf.FloorToInt((currentTime * 100F) % 100F);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void ResetLapData()
    {
        lapCount = 0;
        lapTime = 0;
        currentTime = 0;
        isTiming = false;
        timerText.text = "00:00:00";
    }

    public void EndLap()
    {
        if (lapCount < maxLaps)
        {
            lapTime = Time.time - startTime;
            lapTimes[lapCount] = lapTime;
            Debug.Log("Lap " + (lapCount + 1) + " Time: " + GetFormattedTime());
            lapCount++;
            StartTimer();  // Start next lap
        }
        else
        {
            StopTimer();  // End race
            Debug.Log("Race Finished!");
        }
    }
}
