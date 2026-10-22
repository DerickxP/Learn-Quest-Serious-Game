using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [Header("Backgrounds")]
    public GameObject BGinicial;
    public GameObject BGHelp;
    public GameObject BGConfig;
    public GameObject Painel;

    [Header("Botões")]
    public GameObject Buttons;
    public GameObject ButtonsConfig;
    public GameObject text;
    public GameObject Back;
    public Text textVol;
    public Text textTutorial;

    [Header("Referencias")]
    AudioManager AM;
    public Mentor MT;
    public LegendaManager LM;

    public void Start()
    {
        AM = AudioManager.Instance;
    }

    public void StartGame()
    {
        if (AM != null)
        {
            StartCoroutine(GameMode.Instance.PlaySFX(AM.buttonsSFX, 1));
        }
        else
        {
            Debug.LogError("AudioManager não encontrado!");
        }
    }


    public void Help()
    {
        AM.PlaySFX(AM.buttonsSFX);
        Buttons.SetActive(false);
        BGinicial.SetActive(false);
        BGHelp.SetActive(true);
        text.SetActive(true);
        Back.SetActive(true);

        MT.Verificacao();

        Debug.Log("Carregando tela de Ajuda!");
    }

    public void Config()
    {
        AM.PlaySFX(AM.buttonsSFX);
        Buttons.SetActive(false);
        BGConfig.SetActive(true);
        ButtonsConfig.SetActive(true);
        textTutorial.enabled = true;
        textVol.enabled = true;
        Painel.SetActive(true);
    }

    public void SairDaConfig()
    {
        AM.PlaySFX(AM.buttonsSFX);
        Buttons.SetActive(true);
        BGConfig.SetActive(false);
        ButtonsConfig.SetActive(false);
        textTutorial.enabled = false;
        textVol.enabled = false;
        Painel.SetActive(false);
    }

    public void BackMenu()
    {
        AM.PlaySFX(AM.buttonsSFX);
        BGHelp.SetActive(false);
        text.SetActive(false);
        Buttons.SetActive(true);
        BGinicial.SetActive(true);
        Back.SetActive(false);

        BGConfig.SetActive(false);
        ButtonsConfig.SetActive(false);
        textTutorial.enabled = false;
        textVol.enabled = false;
        LM.ResetCoroutine();
        Painel.SetActive(false);
    }

    public void QuitGame()
    {
        AM.PlaySFX(AM.buttonsSFX);
        Application.Quit();
        Debug.Log("Você quitou do game!");
    }
}
