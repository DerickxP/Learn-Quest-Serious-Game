using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [Header("Objetos da Cena")]
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject CPbuttons;
    [SerializeField] private GameObject BgImage;
    [SerializeField] private GameObject HintButton;

    private Generaltimer timer;

    void Start()
    {
        timer = FindObjectOfType<Generaltimer>();
    }

    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);

        if (currentIndex == 7)
        {
            SceneManager.LoadScene(21);
        }
    }

    public void ScenePrototipo()
    {
        SceneManager.LoadScene(21);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        BgImage.SetActive(true);
        //timer.PauseTimer();
        if (HintButton != null)
        {
            HintButton.SetActive(false);
        }
    }

    public void PauseGameCorrectPronunciation()
    {
        CPbuttons.SetActive(false);
        BgImage.SetActive(true);
        //timer.PauseTimer(); 
    }

    public void BackGame()
    {
        PauseButton.SetActive(true);
        BgImage.SetActive(false);
       // timer.ResumeTimer();
        if (HintButton != null)
        {
            HintButton.SetActive(true);
        }
    }

    public void BackGameCorrectPronunciation()
    {
        PauseButton.SetActive(true);
        BgImage.SetActive(false);
        CPbuttons.SetActive(true);
        //timer.ResumeTimer();
    }

    public void GameModeMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void quitgame()
    {
        Application.Quit();
    }

    public void selectOtherPerson()
    {
        SceneManager.LoadScene(2);
    }
}
