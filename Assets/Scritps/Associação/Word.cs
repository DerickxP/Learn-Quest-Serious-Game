using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Word : MonoBehaviour
{
    public string palavraCorreta;
    private ColisorLetters[] referencias;
    public GameObject painelDerrota;
    public GameObject painelVitoria;
    public List<GameObject> vidasImagens = new List<GameObject>();
    private int tentativasErradas = 0;

    private int faseAtual = 10;

    void Start()
    {
        palavraCorreta = palavraCorreta.ToUpper();
        DefinirLetraEsperadaReferencias();
        painelDerrota.SetActive(false);
        painelVitoria.SetActive(false);
    }

    public void DefinirLetraEsperadaReferencias()
    {
        for (int i = 0; i < referencias.Length; i++)
        {
            if (referencias[i] != null && i < palavraCorreta.Length)
            {
                referencias[i].SetLetraEsperada(palavraCorreta[i].ToString());
            }
        }
    }

    public void VerificarPalavra()
    {
        string palavraFormada = "";
        bool todasReferenciasPreenchidas = true;

        for (int i = 0; i < referencias.Length; i++)
        {
            ColisorLetters referencia = referencias[i];
            if (referencia.LetraColocada())
            {
                palavraFormada += referencia.GetLetraAtual().ToUpper();
            }
            else
            {
                todasReferenciasPreenchidas = false;
                break;
            }
        }

        if (todasReferenciasPreenchidas)
        {
            Debug.Log($"Palavra formada: '{palavraFormada}', Palavra correta: '{palavraCorreta}'");
            if (palavraFormada.ToUpper() == palavraCorreta.ToUpper())
            {
                Debug.Log("Parabéns! Palavra correta.");
                ExibirPainelVitoria();
            }
            else
            {
                Debug.Log("Palavra incorreta! Reiniciando letras.");
                foreach (ColisorLetters referencia in referencias)
                {
                    referencia.LimparReferencia();
                }
                FindObjectOfType<Letters>().ReiniciarLetras();
                TentativaErrada();
            }
        }
    }

    private void ExibirPainelVitoria()
    {
        painelVitoria.SetActive(true);
        Debug.Log("Você venceu! Palavra correta.");
    }

    private void TentativaErrada()
    {
        tentativasErradas++;

        if (tentativasErradas <= vidasImagens.Count)
        {
            vidasImagens[tentativasErradas - 1].SetActive(false);
        }

        if (tentativasErradas >= 3)
        {
            ExibirPainelDerrota();
        }
    }

    private void ExibirPainelDerrota()
    {
        painelDerrota.SetActive(true);
        Debug.Log("Você perdeu! Tentativas esgotadas.");
    }

    public void DefinirReferencias(ColisorLetters[] novasReferencias)
    {
        referencias = novasReferencias;
        DefinirLetraEsperadaReferencias();
        Debug.Log("Referências atualizadas com sucesso.");
    }

    public void PassarDeFase()
    {
        faseAtual++;
        if (faseAtual == 14)
        {
            SceneManager.LoadScene(21);
        }
        else if (faseAtual <= 14)
        {
            SceneManager.LoadScene(faseAtual);
        }
        else
        {
            Debug.LogWarning("Índice de cena fora do intervalo esperado.");
        }
    }

}
