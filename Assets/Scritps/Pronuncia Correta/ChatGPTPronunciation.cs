using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

public class ChatGPTPronunciation : MonoBehaviour
{
    private string openAiApiKey => ApiConfig.OpenAIKey;
    private PronunciationManager pronunciationManager;
    private string palavraCorreta;

    private void Start()
    {
        pronunciationManager = FindObjectOfType<PronunciationManager>();
    }

    public void SetPalavraCorreta(string palavra)
    {
        palavraCorreta = palavra.Trim().ToLower();
        Debug.Log($"Palavra correta definida: {palavraCorreta}");
    }

    public IEnumerator EscolherAnimalAleatorio()
    {
        string prompt = "Escolha um animal aleatorio e comum porém que seja o nome correto e responda apenas o nome.";
        string jsonRequest = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"system\", \"content\": \"Você é um assistente que fornece nomes de animais aleatórios.\"}, {\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"max_tokens\": 10}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + openAiApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            string nomeAnimal = ExtrairNomeAnimal(response);
            pronunciationManager.SetAnimalName(nomeAnimal);
        }
        else
        {
            Debug.LogError("Erro na API ChatGPT ao escolher animal: " + request.error);
        }
    }

    private string ExtrairNomeAnimal(string jsonResponse)
    {
        JObject json = JObject.Parse(jsonResponse);
        string content = json["choices"]?[0]?["message"]?["content"]?.ToString();
        string nomeAnimal = content?.Trim() ?? "animal";

        // Limpar caracteres especiais
        nomeAnimal = LimparNomeAnimal(nomeAnimal);
        return nomeAnimal;
    }

    private string LimparNomeAnimal(string nome)
    {
        // Remover caracteres especiais e manter apenas letras e espaços
        return Regex.Replace(nome, @"[^a-zA-Z\s]", "").Trim();
    }

    public IEnumerator VerificarComChatGPT(string palavraReconhecida)
    {
        // Normalizando a palavra reconhecida
        string palavraProcessada = palavraReconhecida.Trim().ToLower();
        Debug.Log($"Palavra reconhecida: {palavraProcessada}, Palavra correta: {palavraCorreta}");

        // Comparação das palavras
        bool acertou = string.Equals(palavraProcessada, palavraCorreta, StringComparison.OrdinalIgnoreCase);
        pronunciationManager.VerificarResultado(acertou);
        yield break;
    }

    public IEnumerator ObterCuriosidadesDoAnimal(string nomeAnimal)
    {
        string prompt = $"Liste 3 curiosidades curtas e simples sobre o animal '{nomeAnimal}' em até 100 palavras.";
        string jsonRequest = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"system\", \"content\": \"Você é um assistente que dá informações curtas e simples sobre animais.\"}, {\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"max_tokens\": 100}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + openAiApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string curiosidades = request.downloadHandler.text;
            string curiositiesText = ExtrairCuriosidades(curiosidades);
            pronunciationManager.ReceiveCuriosities(curiositiesText);
        }
        else
        {
            Debug.LogError("Erro na API ChatGPT ao buscar curiosidades: " + request.error);
        }
    }

    private string ExtrairCuriosidades(string jsonResponse)
    {
        JObject json = JObject.Parse(jsonResponse);
        string content = json["choices"]?[0]?["message"]?["content"]?.ToString();
        return content ?? "Sem curiosidades disponíveis.";
    }
}
