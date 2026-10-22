using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class DallERequest : MonoBehaviour
{
    private string apiKey => ApiConfig.OpenAIKey;
    private string apiUrl = "https://api.openai.com/v1/images/generations";
    public Image imageDisplay;

    public void GenerateCartoonAnimal(string animalName)
    {
        StartCoroutine(RequestDallE(animalName));
    }

    private IEnumerator RequestDallE(string animalName)
    {
        string prompt = $"{animalName} cartoon";
        string jsonRequest = "{\"prompt\": \"" + prompt + "\", \"n\": 1, \"size\":\"512x512\"}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            string imageUrl = ExtractImageUrl(jsonResponse);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                StartCoroutine(DownloadImage(imageUrl));
            }
            else
            {
                Debug.LogError("URL da imagem não encontrada na resposta.");
            }
        }
        else
        {
            Debug.LogError("Erro ao gerar imagem no DALL-E: " + request.error);
            Debug.LogError("Resposta: " + request.downloadHandler.text); // Adiciona mais contexto sobre o erro.
        }
    }

    private string ExtractImageUrl(string jsonResponse)
    {
        JObject json = JObject.Parse(jsonResponse);
        string imageUrl = json["data"]?[0]?["url"]?.ToString();
        return imageUrl;
    }

    private IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            imageDisplay.sprite = sprite;
        }
        else
        {
            Debug.LogError("Erro ao baixar a imagem do DALL-E: " + request.error);
        }
    }
}
