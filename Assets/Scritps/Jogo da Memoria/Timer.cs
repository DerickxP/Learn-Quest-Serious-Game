using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 70;
    public bool timerIsRunning = false;
    public Slider timeSlider;
    private bool isPaused = false;
    
    [SerializeField]
    private MemoryGame MG;


    void Start()
    {
        timerIsRunning = true;
        timeSlider.maxValue = timeRemaining;
        timeSlider.value = timeRemaining;
    }

    void Update()
    {
        if (timerIsRunning && !isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
                timeSlider.value = timeRemaining;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                EndGame();
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        timeSlider.value = currentTime;
    }

    void EndGame()
    {
        Debug.Log("Tempo esgotado! Fim de jogo.");
        MG.GameOverPanel.SetActive(true);
    }

    public void TogglePause(bool shouldPause)
    {
        isPaused = shouldPause;
        timerIsRunning = !shouldPause;
        Debug.Log("Timer pausado: " + isPaused);
    }

    public void ResetTimer(float newTime = 60f)
    {
        timeRemaining = newTime;
        timerIsRunning = true;
        isPaused = false;
        timeSlider.maxValue = timeRemaining;
        timeSlider.value = timeRemaining;
    }

    public void AddTime(float amount)
    {
        timeRemaining += amount;
        timeSlider.value = timeRemaining;
    }

    public void SubtractTime(float amount)
    {
        timeRemaining -= amount;
        if (timeRemaining < 0) timeRemaining = 0;
        timeSlider.value = timeRemaining;
    }
}
