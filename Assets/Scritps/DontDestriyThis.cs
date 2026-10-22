using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestriyThis : MonoBehaviour
{
    /* 
       Scrpit para gerenciar a coroutine de audio, com ela se permite a continuação 
       do audio inicial do menu, até a escolha de fases.
   */

    [Header("Referencias")]
    public GameObject AudioContinuo;
    // Start is called before the first frame update
    void Start()
    {
            DontDestroyOnLoad(AudioContinuo);

    }

}
