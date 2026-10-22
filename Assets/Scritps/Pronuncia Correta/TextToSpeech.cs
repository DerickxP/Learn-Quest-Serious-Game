using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class TextToSpeech : MonoBehaviour
{
    private string apiKey => ApiConfig.GoogleKey;

    // Função pública para reprodução de áudio
    public void PlayAudioFromText(string inputText)
    {
        StartCoroutine(SynthesizeSpeech(inputText));
    }

    // Faz a solicitação para a API do Google TTS
    IEnumerator SynthesizeSpeech(string inputText)
    {
        string url = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + apiKey;
        string jsonPayload = "{\"input\":{\"text\":\"" + inputText + "\"},\"voice\":{\"languageCode\":\"pt-BR\",\"ssmlGender\":\"FEMALE\"},\"audioConfig\":{\"audioEncoding\":\"LINEAR16\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Erro na requisição TTS: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            var response = JsonUtility.FromJson<GoogleTTSResponse>(jsonResponse);
            byte[] audioData = System.Convert.FromBase64String(response.audioContent);
            PlayAudioClip(audioData);
        }
    }

    // Converte o áudio e o reproduz
    private void PlayAudioClip(byte[] audioData)
    {
        AudioClip clip = WavUtility.ToAudioClip(audioData);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    [System.Serializable]
    public class GoogleTTSResponse
    {
        public string audioContent;
    }
}
