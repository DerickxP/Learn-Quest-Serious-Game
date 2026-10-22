using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LegendaManager : MonoBehaviour
{
    [Header("Instructions")]
    public Text sentenceText;

    [SerializeField] Mentor currentDialogue;
    [SerializeField] private float typingSpeed;
    private int currentIndex;
    private string[] sentences;

    [Header("Buttons")]
    public Button nextButton;

    private bool isTyping = false;

    void Start()
    {
        if (nextButton == null)
        {
            Debug.LogError("Botão de próximo não está atribuído.");
            return;
        }

        if (sentenceText == null)
        {
            Debug.LogError("Texto da frase não está atribuído.");
            return;
        }
        nextButton.onClick.AddListener(OnNextButtonClicked);
        Debug.Log("Botão conectado ao evento.");
    }

    public void Speak(string[] txt, Mentor current)
    {
        if (txt == null || txt.Length == 0)
        {
            Debug.LogError("Nenhuma frase fornecida.");
            return;
        }

        sentences = txt;
        currentDialogue = current;

        ResetCoroutine();
        currentIndex = 0;

        if (sentences.Length > 0) // Verificar se há frases para exibir
        {
            StartCoroutine(TypeSentence());
            nextButton.interactable = true;
        }
        else
        {
            Debug.LogError("Nenhuma frase foi inicializada.");
        }
    }

    IEnumerator TypeSentence()
    {
        if (sentences == null)
        {
            Debug.LogError("Sentences não está inicializado.");
            yield break;
        }
        if (currentIndex >= sentences.Length)
        {
            Debug.LogError("currentIndex está fora dos limites do array sentences.");
            yield break;
        }
        if (sentenceText == null)
        {
            Debug.LogError("Texto da frase não está atribuído.");
            yield break;
        }

        isTyping = true;
        sentenceText.text = "";

        foreach (char letter in sentences[currentIndex].ToCharArray())
        {
            sentenceText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        Debug.Log("Frase concluída.");
    }

    public void NextSentence()
    {
        if (currentIndex < sentences.Length - 1)
        {
            currentIndex++;
            StartCoroutine(TypeSentence());
        }
        else
        {
            nextButton.interactable = false;
            Debug.Log("Nenhuma frase restante. Botão desativado.");
        }
    }

    public void ResetCoroutine()
    {
        StopAllCoroutines();
        sentenceText.text = "";
        isTyping = false;
        Debug.Log("Coroutine resetada.");
    }

    public void OnNextButtonClicked()
    {
        Debug.Log("Botão clicado!");

        if (isTyping)
        {
            StopAllCoroutines();
            if (sentences != null && currentIndex < sentences.Length)
            {
                sentenceText.text = sentences[currentIndex];
            }
            isTyping = false;
            Debug.Log("Frase atual completada imediatamente.");
        }
        if (!isTyping)
        {
            if (currentIndex < sentences.Length - 1)
            {
                NextSentence();
            }
            else
            {
                nextButton.interactable = false;
                Debug.Log("Última frase exibida. Botão desativado.");
            }
        }
    }
}
