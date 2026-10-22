using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GravadorAudio : MonoBehaviour
{
    public Button botaoMicrofone;
    public Image imagemMicrofone;
    private AudioClip gravacao;
    private bool gravando = false;
    private Coroutine piscarCoroutine;

    void Start()
    {
        botaoMicrofone.onClick.AddListener(ToggleGravacao);
    }

    void ToggleGravacao()
    {
        if (!gravando)
        {
            gravacao = Microphone.Start(null, false, 5, 44100);
            gravando = true;
            Debug.Log("Gravação iniciada...");
            piscarCoroutine = StartCoroutine(PiscarMicrofone());
        }
        else
        {
            Microphone.End(null);
            gravando = false;
            Debug.Log("Gravação encerrada.");
            if (piscarCoroutine != null)
            {
                StopCoroutine(piscarCoroutine);
            }
            imagemMicrofone.color = new Color(imagemMicrofone.color.r, imagemMicrofone.color.g, imagemMicrofone.color.b, 1f);

            byte[] audioBytes = WavUtility.FromAudioClip(gravacao);
            GoogleSpeechToText googleSpeechToText = gameObject.AddComponent<GoogleSpeechToText>();
            StartCoroutine(googleSpeechToText.EnviarAudioGoogleAPI(audioBytes));
        }
    }

    IEnumerator PiscarMicrofone()
    {
        float tempoVisivel = 0.4f;
        float tempoInvisivel = 0.2f; 
        float duracaoTransicao = 0.8f;
        float opacidadeMinima = 0.2f; 

        while (gravando)
        {
            yield return FadeMicrofone(1f, opacidadeMinima, duracaoTransicao); 
            yield return new WaitForSeconds(tempoInvisivel);

            yield return FadeMicrofone(opacidadeMinima, 1f, duracaoTransicao); 
            yield return new WaitForSeconds(tempoVisivel);
        }
    }


    IEnumerator FadeMicrofone(float deOpacidade, float paraOpacidade, float duracao)
    {
        float tempoInicial = Time.time;
        float opacidadeAtual = imagemMicrofone.color.a;

        while (Time.time - tempoInicial < duracao)
        {
            float t = (Time.time - tempoInicial) / duracao; 
            float novaOpacidade = Mathf.SmoothStep(deOpacidade, paraOpacidade, t);
            imagemMicrofone.color = new Color(imagemMicrofone.color.r, imagemMicrofone.color.g, imagemMicrofone.color.b, novaOpacidade);
            yield return null;
        }

        imagemMicrofone.color = new Color(imagemMicrofone.color.r, imagemMicrofone.color.g, imagemMicrofone.color.b, paraOpacidade);
    }
}
