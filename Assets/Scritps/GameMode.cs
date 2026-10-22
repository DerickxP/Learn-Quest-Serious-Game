using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMode : MonoBehaviour
{
    AudioManager AM;
    public static GameMode Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AM = AudioManager.Instance;
    }

    public void Labirinto()
    {
        AM.Stop();
        StartCoroutine(PlaySFX(AM.buttonsSFX, 2));
    }

    public void Associacao()
    {
        StartCoroutine(PlaySFX(AM.buttonsSFX, 10));
        AM.Stop();
    }

    public void MemoriaInterativa()
    {
        AM.Stop();
        StartCoroutine(PlaySFX(AM.buttonsSFX, 8));
    }
    public void PronunciaCorreta()
    {
        AM.Stop();
        StartCoroutine(PlaySFX(AM.buttonsSFX, 16));
    }

    public IEnumerator PlaySFX(AudioClip buttonsSFX, int sceneIndex) 
    {
        AM.PlaySFX(buttonsSFX);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneIndex);
    }
}
