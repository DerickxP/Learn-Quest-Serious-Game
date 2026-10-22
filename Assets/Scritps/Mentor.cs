using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mentor : MonoBehaviour
{
    [Header("Instruções")]
    public string[] sentenceText; //Frases pras intruções do jogo.


    [Header("Referências")]
    private LegendaManager LM;


    public void Start()
    {
        LM = FindObjectOfType<LegendaManager>();
    }

    public void Verificacao()
    {
        LM.Speak(sentenceText, this); // Verifica a inicialização da digitação referente a legenda do tutorial.
    }
}
