using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class GoogleSpeechToText : MonoBehaviour
{
    private string apiKey => ApiConfig.GoogleKey;
    private ChatGPTPronunciation chatGptScript;

    void Start()
    {
        chatGptScript = FindObjectOfType<ChatGPTPronunciation>();
    }

    public IEnumerator EnviarAudioGoogleAPI(byte[] audioData)
    {
        string url = "https://speech.googleapis.com/v1/speech:recognize?key=" + apiKey;

        string audioBase64 = System.Convert.ToBase64String(audioData);
        string jsonRequest = "{\"config\":{\"encoding\":\"LINEAR16\",\"sampleRateHertz\":44100,\"languageCode\":\"pt-BR\"},\"audio\":{\"content\":\"" + audioBase64 + "\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Resposta da API: " + request.downloadHandler.text);
            ProcessarRespostaGoogle(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Erro na API: " + request.error);
        }
    }

    void ProcessarRespostaGoogle(string jsonResponse)
    {
        string palavraReconhecida = ExtrairPalavraReconhecida(jsonResponse);
        Debug.Log("Palavra reconhecida: " + palavraReconhecida);

        if (chatGptScript != null)
        {
            StartCoroutine(chatGptScript.VerificarComChatGPT(palavraReconhecida));
        }
        else
        {
            Debug.LogError("Script ChatGPT não encontrado!");
        }
    }

    string ExtrairPalavraReconhecida(string jsonResponse)
    {
        JObject json = JObject.Parse(jsonResponse);
        string palavra = json["results"]?[0]?["alternatives"]?[0]?["transcript"]?.ToString();
        Debug.Log($"Palavra extraída: {palavra}");
        return palavra?.Trim().ToLower() ?? string.Empty;
    }

}
