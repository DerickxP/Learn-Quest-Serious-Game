using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image loadingImage;
    public Text loadingText;
    public string[] loadingMessages;
    private int currentMessageIndex = 0;
    public float rotationSpeed = 10f;
    public float messageInterval = 2.5f;

    public GameObject panelToDisable;  // Painel a ser desativado
    public float fadeOutDuration = 10f; // Duração do fade-out em segundos

    private CanvasGroup panelCanvasGroup; // Para controlar o fade-out do painel

    private void Start()
    {
        // Verifica se há mensagens a serem exibidas
        if (loadingMessages.Length > 0)
        {
            StartCoroutine(UpdateMessages());
        }

        // Obtém ou adiciona um CanvasGroup ao painel para gerenciar sua opacidade
        if (panelToDisable != null)
        {
            panelCanvasGroup = panelToDisable.GetComponent<CanvasGroup>();
            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = panelToDisable.AddComponent<CanvasGroup>();
            }
            panelCanvasGroup.alpha = 1f; // Garante que o painel comece totalmente visível
            StartCoroutine(FadeOutPanel());
        }
    }

    private void Update()
    {
        RotateLoadingImage();
    }

    private void RotateLoadingImage()
    {
        if (loadingImage != null)
        {
            loadingImage.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator UpdateMessages()
    {
        while (true)
        {
            loadingText.text = loadingMessages[currentMessageIndex];
            currentMessageIndex = (currentMessageIndex + 1) % loadingMessages.Length;
            yield return new WaitForSeconds(messageInterval);
        }
    }

    private IEnumerator FadeOutPanel()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        // Após o fade-out, desativa o painel
        panelCanvasGroup.alpha = 0f;
        if (panelToDisable != null)
        {
            panelToDisable.SetActive(false);
        }
    }
}
