using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PronunciationManager : MonoBehaviour
{
    [Header("Variáveis Volume")]
    private bool audioOn = false;
    [SerializeField] Image PauseOn;
    [SerializeField] Image PlayOn;

    [Header("Game Manager")]
    public GameObject imageDisplay;
    private AudioSource audioSource;
    private ChatGPTPronunciation chatGptScript;
    private TextToSpeech googleTTS; 
    private DallERequest dallEScript;
    private string currentAnimalName;

    [Header("Tela de Vitória")]
    public GameObject victoryPanel;
    public TMP_Text curiositiesText;
    public Button nextAnimalButton;
    private int faseAtual = 16;

    [Header("Tela de Aviso e Derrota")]
    public GameObject warningPanel; 
    public GameObject defeatPanel;  
    private int errorCount = 0;    

    private void Start()
    {
        PauseOn.enabled = false;
        audioSource = GetComponent<AudioSource>();
        chatGptScript = FindObjectOfType<ChatGPTPronunciation>();
        googleTTS = GetComponent<TextToSpeech>();
        dallEScript = FindObjectOfType<DallERequest>();
        RespawnRandomAnimal();

        victoryPanel.SetActive(false);
        warningPanel.SetActive(false); 
        defeatPanel.SetActive(false);  
        nextAnimalButton.onClick.AddListener(NextAnimal);

        PronunciationManager[] managers = FindObjectsOfType<PronunciationManager>();
        Debug.Log("Total de GameObjects usando PronunciationManager: " + managers.Length);
    }

    public void playAudio()
    {
        if (googleTTS != null && !string.IsNullOrEmpty(currentAnimalName))
        {
            googleTTS.PlayAudioFromText(currentAnimalName);
        }
        else
        {
            Debug.LogError("Google TTS ou nome do animal não disponível.");
        }
    }

    void Update()
    {
        if (audioSource != null && !audioSource.isPlaying && audioOn)
        {
            PauseOn.enabled = false;
            PlayOn.enabled = true;
            audioOn = false;
        }
    }

    void RespawnRandomAnimal()
    {
        if (chatGptScript != null)
        {
            StartCoroutine(chatGptScript.EscolherAnimalAleatorio());
        }
        else
        {
            Debug.LogError("Script ChatGPT não encontrado!");
        }
    }

    public void SetAnimalName(string nomeAnimal)
    {
        currentAnimalName = nomeAnimal;
        Debug.Log($"Animal escolhido pelo ChatGPT: {currentAnimalName}");
        chatGptScript.SetPalavraCorreta(currentAnimalName);
        dallEScript.GenerateCartoonAnimal(currentAnimalName);
    }

    public void VerificarResultado(bool acertou)
    {
        if (acertou)
        {
            Debug.Log("Palavra correta! Obtendo curiosidades do ChatGPT...");
            StartCoroutine(chatGptScript.ObterCuriosidadesDoAnimal(currentAnimalName));
            errorCount = 0;
        }
        else
        {
            Debug.Log("Palavra incorreta! Tente novamente.");
            errorCount++;

            if (errorCount >= 4)
            {
                ShowDefeatScreen();
            }
            else
            {
                StartCoroutine(ShowWarningPanel()); 
            }
        }
    }

    public void ReceiveCuriosities(string curiosities)
    {
        ShowVictoryScreen(curiosities);
    }

    private void ShowVictoryScreen(string curiosities)
    {
        victoryPanel.SetActive(true);
        curiositiesText.text = curiosities;
    }

    private void NextAnimal()
    {
        faseAtual++;
        if (faseAtual == 20)
        {
            SceneManager.LoadScene(21);
        }
        else if (faseAtual <= 20)
        {
            SceneManager.LoadScene(faseAtual);
        }
        else
        {
            Debug.LogWarning("Índice de cena fora do intervalo esperado.");
        }
    }

    private IEnumerator ShowWarningPanel()
    {
        warningPanel.SetActive(true);
        yield return new WaitForSeconds(3); 
        warningPanel.SetActive(false);  
    }

    private void ShowDefeatScreen()
    {
        defeatPanel.SetActive(true);
    }
}
