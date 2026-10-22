using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Letters : MonoBehaviour
{
    [Header("Variaveis Publicas")]
    public ChatGPT gptIntegration;
    public GameObject letraPrefab;
    public Transform letrasRespawn;
    public GameObject referenciaPrefab;
    public Transform pontoRespawnReferencias;
    public float espacamentoEntreReferencias = 1.5f;
    public float espacamentoEntreLetras = 0.5f;
    public float snapDistance = 0.5f;

    [Header("Listas")]
    public List<ClassLetter> letrasAlfabeto;
    private List<GameObject> letrasInstanciadas = new List<GameObject>();
    private List<GameObject> referenciasInstanciadas = new List<GameObject>();

    public void Start()
    {
        StartCoroutine(GetAnimal());
    }

    public IEnumerator GetAnimal()
    {
        var animal = gptIntegration.GetAnimalAsync();
        yield return new WaitUntil(() => animal.IsCompleted);

        OnAnimalReceived(animal.Result);
    }

    private void OnAnimalReceived(Animal animalData)
    {
        if (animalData != null)
        {
            FindObjectOfType<Word>().palavraCorreta = animalData.Nome;
            Debug.Log("Palavra correta atribuída: " + animalData.Nome);
            RespawnLetras(animalData.Nome);
            RespawnReferencias(animalData.Nome);
            FindObjectOfType<Word>().DefinirLetraEsperadaReferencias();
        }
    }

    private void RespawnReferencias(string palavra)
    {
        foreach (var referencia in referenciasInstanciadas)
        {
            Destroy(referencia);
        }
        referenciasInstanciadas.Clear();

        float larguraTotalReferencias = (palavra.Length - 1) * espacamentoEntreReferencias;
        Vector3 posicaoInicialReferencias = pontoRespawnReferencias.position - new Vector3(larguraTotalReferencias / 2, 0, 0);

        for (int i = 0; i < palavra.Length; i++)
        {
            Vector3 posicaoReferencia = posicaoInicialReferencias + new Vector3(i * espacamentoEntreReferencias, 0, 0);
            GameObject referenciaGO = Instantiate(referenciaPrefab, posicaoReferencia, Quaternion.identity);
            referenciasInstanciadas.Add(referenciaGO);
        }

        FindObjectOfType<Word>().DefinirReferencias(referenciasInstanciadas.Select(r => r.GetComponent<ColisorLetters>()).ToArray());
    }

    private void RespawnLetras(string palavra)
    {
        foreach (var letra in letrasInstanciadas)
        {
            Destroy(letra);
        }
        letrasInstanciadas.Clear();

        Dictionary<char, int> frequenciaLetras = new Dictionary<char, int>();
        foreach (char letra in palavra.ToUpper())
        {
            if (frequenciaLetras.ContainsKey(letra))
            {
                frequenciaLetras[letra]++;
            }
            else
            {
                frequenciaLetras[letra] = 1;
            }
        }

        List<ClassLetter> letrasDisponiveis = new List<ClassLetter>();
        int numLetras = palavra.Length;
        int totalLetras = Mathf.Max(6, numLetras);
        float larguraTotalLetras = (totalLetras - 1) * espacamentoEntreLetras;
        Vector3 posicaoInicialLetras = letrasRespawn.position - new Vector3(larguraTotalLetras / 2, 0, 0);

        int indexLetra = 0;
        foreach (var item in frequenciaLetras)
        {
            char letra = item.Key;
            int quantidade = item.Value;

            ClassLetter letraAlfabeto = letrasAlfabeto.FirstOrDefault(l => l.letra.ToUpper() == char.ToUpper(letra).ToString());

            if (letraAlfabeto != null)
            {
                letrasDisponiveis.Add(letraAlfabeto);
                for (int i = 0; i < quantidade; i++)
                {
                    GameObject letraGO = Instantiate(letraPrefab, posicaoInicialLetras + Vector3.right * (indexLetra * espacamentoEntreLetras), Quaternion.identity);
                    letraGO.GetComponent<SpriteRenderer>().sprite = letraAlfabeto.sprite;
                    letraGO.name = letraAlfabeto.letra.ToUpper();
                    letrasInstanciadas.Add(letraGO);
                    indexLetra++;
                }
            }
        }

        while (indexLetra < 6)
        {
            ClassLetter letraAleatoria = letrasAlfabeto[Random.Range(0, letrasAlfabeto.Count)];
            if (!letrasDisponiveis.Contains(letraAleatoria))
            {
                letrasDisponiveis.Add(letraAleatoria);
                GameObject letraGO = Instantiate(letraPrefab, posicaoInicialLetras + Vector3.right * (indexLetra * espacamentoEntreLetras), Quaternion.identity);
                letraGO.GetComponent<SpriteRenderer>().sprite = letraAleatoria.sprite;
                letraGO.name = letraAleatoria.letra.ToUpper();
                letrasInstanciadas.Add(letraGO);
                indexLetra++;
            }
        }

        EmbaralharLetras();
    }

    public void ReiniciarLetras()
    {
        foreach (GameObject letra in letrasInstanciadas)
        {
            letra.transform.position = letrasRespawn.position;
        }
        EmbaralharLetras();
    }

    private void EmbaralharLetras()
    {
        System.Random rng = new System.Random();
        int n = letrasInstanciadas.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject temp = letrasInstanciadas[k];
            letrasInstanciadas[k] = letrasInstanciadas[n];
            letrasInstanciadas[n] = temp;
        }

        for (int i = 0; i < letrasInstanciadas.Count; i++)
        {
            letrasInstanciadas[i].transform.position = letrasRespawn.position + Vector3.right * (i * espacamentoEntreLetras);
        }
    }
}
