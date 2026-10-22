using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoImagem : MonoBehaviour
{
    public float velocidadeSubindo = 15f;
    public float distanciaSubida = 30f;
    public float duracaoDesaparecendo = 2f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 posicaoInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        posicaoInicial = rectTransform.position;
        rectTransform.position = new Vector3(posicaoInicial.x, -1000f, posicaoInicial.z);
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false); 
    }

    public IEnumerator AnimarImagem()
    {
        gameObject.SetActive(true);  
        canvasGroup.alpha = 1f;
        rectTransform.position = posicaoInicial; 

        float tempoSubida = 0f;
        Vector3 destinoSubida = posicaoInicial + Vector3.up * distanciaSubida;
        while (rectTransform.position.y < destinoSubida.y)
        {
            rectTransform.position += Vector3.up * velocidadeSubindo * Time.deltaTime;
            tempoSubida += velocidadeSubindo * Time.deltaTime;
            yield return null;
        }

        rectTransform.position = destinoSubida; 
        yield return StartCoroutine(Desaparecer());
    }

    IEnumerator Desaparecer()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / duracaoDesaparecendo;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);  
    }
}
