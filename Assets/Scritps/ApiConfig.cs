using UnityEngine;

// Carrega as chaves de API de Assets/Resources/ApiKeys.json, arquivo que fica fora do git.
// Para configurar, copie ApiKeys.example.json (raiz do projeto) para Assets/Resources/ApiKeys.json
// e preencha com as suas chaves.
public static class ApiConfig
{
    [System.Serializable]
    private class ApiKeys
    {
        public string openAiApiKey;
        public string googleCloudApiKey;
    }

    private static ApiKeys keys;

    public static string OpenAIKey => Carregar().openAiApiKey;
    public static string GoogleKey => Carregar().googleCloudApiKey;

    private static ApiKeys Carregar()
    {
        if (keys != null)
        {
            return keys;
        }

        TextAsset arquivo = Resources.Load<TextAsset>("ApiKeys");
        if (arquivo == null)
        {
            Debug.LogError("ApiKeys.json não encontrado em Assets/Resources. Veja ApiKeys.example.json na raiz do projeto.");
            keys = new ApiKeys();
            return keys;
        }

        keys = JsonUtility.FromJson<ApiKeys>(arquivo.text);
        return keys;
    }
}
