using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ChatGPT : MonoBehaviour
{
    private static string apiKey => ApiConfig.OpenAIKey;
    private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    public GameManager animalManager;
    public DallERequestAssociation dallERequest;

    public async Task<Animal> GetAnimalAsync()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            string animalNome = await GetAnimalFromGPT(client);
            if (string.IsNullOrEmpty(animalNome) || animalManager.AnimalJaSelecionado(animalNome))
                return null;

            animalManager.AnimalSelecionado(animalNome);
            dallERequest.GenerateCartoonAnimal(animalNome);

            return new Animal(animalNome, (string)null);
        }
    }

    private async Task<string> GetAnimalFromGPT(HttpClient client)
    {
        var requestData = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = "Forneça o nome aleatório de um animal silvestre ou domestico, não coloque animais com nome composto, nem animais pre historicos." }
            },
            max_tokens = 50
        };

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(apiUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError("Erro ao buscar animal: " + response.ReasonPhrase);
            return null;
        }

        var result = await response.Content.ReadAsStringAsync();
        Debug.Log("Resposta da API: " + result);
        return ExtractAnimalFromResponse(result);
    }

    private string ExtractAnimalFromResponse(string response)
    {
        try
        {
            var jsonResponse = JObject.Parse(response);
            var choices = jsonResponse["choices"];
            if (choices != null && choices.HasValues)
            {
                string animal = choices[0]["message"]["content"].ToString().Trim();
                animal = animal.TrimEnd('.', ',', '!', '?', '~', '`', '´');
                return animal;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erro ao extrair o animal da resposta: " + ex.Message);
        }
        return null;
    }

}
