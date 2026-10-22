using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class DallERequestAssociation : MonoBehaviour
{
    private string apiKey => ApiConfig.OpenAIKey;
    private string apiUrl = "https://api.openai.com/v1/images/generations";
    public RawImage imageDisplay;
    public void GenerateCartoonAnimal(string animalName)
    {
        StartCoroutine(RequestDallE(animalName));
    }

    private IEnumerator RequestDallE(string animalName)
    {
        string prompt = $"{animalName} cartoon";
        string jsonRequest = "{\"prompt\": \"" + prompt + "\", \"n\": 1, \"size\":\"256x256\"}";

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
            Debug.LogError("Erro ao buscar imagem: " + request.error);
        }
    }

    private string ExtractImageUrl(string response)
    {
        try
        {
            var json = JObject.Parse(response);
            return json["data"][0]["url"].ToString();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erro ao extrair a URL da imagem: " + ex.Message);
            return null;
        }
    }

    private IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            imageDisplay.texture = texture;
            RectTransform rectTransform = imageDisplay.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 300);
            imageDisplay.SetNativeSize();
        }
        else
        {
            Debug.LogError("Erro ao baixar a imagem: " + request.error);
        }
    }


} 