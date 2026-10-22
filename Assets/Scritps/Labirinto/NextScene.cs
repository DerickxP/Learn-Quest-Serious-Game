using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    // Objeto onde será instanciado o ícone selecionado
    public GameObject objectToInstantiateIcon;
    public GameObject victoryCondition;

    void Start()
    {

        // Recupera o índice do ícone selecionado
        int Index = PlayerPrefs.GetInt("IndexIconSelected", -1);

        // Verifica se um ícone foi selecionado
        if (Index != -1)
        {
            // Recupera o prefab do ícone com base no índice
            GameObject prefabDoIcone = FindObjectOfType<ManagerLabirinto>().SelecionarIconePorIndice(Index);

            // Recupera o prefab da condição de vitoria com base no índice
            GameObject prefabVictoryCondition = FindObjectOfType<ManagerLabirinto>().SelecionarCondicaoPorIndice(Index);

            // Instancia o ícone na cena
            if (prefabDoIcone != null && prefabVictoryCondition != null)
            {
                Instantiate(prefabDoIcone, objectToInstantiateIcon.transform.position, Quaternion.identity);
                Instantiate(prefabVictoryCondition, victoryCondition.transform.position, Quaternion.identity);

            }
            else
            {
                Debug.LogError("Ícone não encontrado");
            }

        }

        else
        {
            Debug.LogError("Índice do ícone não encontrado");
        }
    }
}
