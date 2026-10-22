using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLabirinto : MonoBehaviour
{
    [Header("Scene Manager")]
    public List<GameObject> iconPrefabs;
    public List<GameObject> iconPrefabsVictory;
    public GameObject TeladeVitoria;

    [Header("Referencias")]
    AudioManager AM;

    [Header("Variaveis")]
    public Transform instacePoint;
    public Transform instaceVictoryPoint;

    void Start()
    {
        AM = AudioManager.Instance;
    }

    public void BotaoCena()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 7)
        {
            SceneManager.LoadScene("CenaEncerramentoPrototipo"); 
            TeladeVitoria.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
            TeladeVitoria.SetActive(false);
        }
    }

    public GameObject SelecionarIconePorIndice(int indice)
    {
        if (indice >= 0 && indice < iconPrefabs.Count)
        {
            return iconPrefabs[indice];
        }
        else
        {
            Debug.LogError("Índice fora do intervalo");
            return null;
        }
    }

    public GameObject SelecionarCondicaoPorIndice(int indice)
    {
        if (indice >= 0 && indice < iconPrefabsVictory.Count)
        {
            return iconPrefabsVictory[indice];
        }
        else
        {
            Debug.LogError("Índice fora do intervalo");
            return null;
        }
    }

    public void SelectOvelha()
    {
        StartCoroutine(PlaySFX());
        PlayerPrefs.SetInt("IndexIconSelected", 0);
        PlayerPrefs.Save();
    }

    public void SelectMacaco()
    {
        StartCoroutine(PlaySFX());
        PlayerPrefs.SetInt("IndexIconSelected", 1);
        PlayerPrefs.Save();
    }

    public void SelectCat()
    {
        StartCoroutine(PlaySFX());
        PlayerPrefs.SetInt("IndexIconSelected", 2);
        PlayerPrefs.Save();
    }

    public void SelectTigre()
    {
        StartCoroutine(PlaySFX());
        PlayerPrefs.SetInt("IndexIconSelected", 3);
        PlayerPrefs.Save();
    }

    public IEnumerator PlaySFX()
    {
        AM.PlaySFX(AM.buttonsSFX);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(3);
    }
}
