using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generaltimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    private bool isRunning = true;

    void Start()
    {
        timeElapsed = 0f; 
    }

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime; 
            UpdateTimerDisplay(timeElapsed);
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60); 

        timerText.text = string.Format("TEMPO: {0:00}:{1:00}", minutes, seconds); 
    }

    public void StartTimer()
    {
        isRunning = true;
        timeElapsed = 0f;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        UpdateTimerDisplay(timeElapsed);
    }
}
