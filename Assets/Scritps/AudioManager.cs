using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Referências")]
    public static AudioManager Instance;
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    [Header("sfxAudios")]
    public AudioClip successSFX;
    public AudioClip buttonsSFX;
    public AudioClip lettersSFX;

    [Header("Músicas de Fundo")]
    public AudioClip menuBGM;
    public AudioClip labirintoBGM;
    public AudioClip associacaoBGM;
    public AudioClip memoriaBGM;
    public AudioClip pronunciaBGM;

    [Header("Variáveis de Volume")]
    private bool somOn = true;
    [SerializeField] private float bgmVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.8f; 
    [SerializeField] Image soundOff;
    [SerializeField] Image soundOn;

    private List<string> menuScenes = new List<string> { "Menu", "GameMode" };
    private List<string> labirintoScenes = new List<string> { "EscolhadosPersonagens", "LAB1", "LAB2", "LAB3", "LAB4", "LAB5" };
    private List<string> AssociacaoScenes = new List<string> { "Fase 01", "Fase 02", "Fase 03", "Fase 04", "Fase 05"};

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusicaSceneAtiva();
        OnOff();
        AjustarVolumes();
    }

    private void PlayMusicaSceneAtiva()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (menuScenes.Contains(sceneName))
        {
            PlayBGM(menuBGM);
        }
        else if (labirintoScenes.Contains(sceneName))
        {
            PlayBGM(labirintoBGM);
        }
        else if (AssociacaoScenes.Contains(sceneName)){

            PlayBGM(associacaoBGM);
        }
        else
        {
            switch (sceneName)
            {
                case "MemoriaInterativa":
                    PlayBGM(memoriaBGM);
                    break;
                case "PronunciaCorreta":
                    PlayBGM(pronunciaBGM);
                    break;
                default:
                    Stop();
                    break;
            }
        }
    }

    public void PlayBGM(AudioClip newMusic)
    {
        if (bgmSource.clip != newMusic || !bgmSource.isPlaying)
        {
            bgmSource.clip = newMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx, sfxVolume); 
    }

    public void Stop()
    {
        bgmSource.Stop();
    }

    public void VolumeDoJogo()
    {
        somOn = !somOn;
        if (somOn)
        {
            PlaySFX(buttonsSFX);
            bgmSource.mute = false;
            sfxSource.mute = false;
        }
        else
        {
            bgmSource.mute = true;
            sfxSource.mute = true;
        }

        OnOff();
    }

    private void OnOff()
    {
        if (soundOn != null && soundOff != null)
        {
            soundOn.enabled = somOn;
            soundOff.enabled = !somOn;
        }
        else
        {
            Debug.LogWarning("As referências 'soundOn' ou 'soundOff' não foram atribuídas no Inspector.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicaSceneAtiva();
    }

    private void AjustarVolumes()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume; 
    }
}
