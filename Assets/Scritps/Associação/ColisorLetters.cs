using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorLetters : MonoBehaviour
{
    public string letraEsperada;
    public GameObject letraAtual;
    private Color corOriginal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Letra"))
        {
            if (!LetraColocada())
            {
                ColocarLetra(other.gameObject);
                FindObjectOfType<Word>().VerificarPalavra(); 
            }
        }
    }

    public void ColocarLetra(GameObject letra)
    {
        letraAtual = letra;
        letra.transform.position = transform.position;
        Collider2D col = letraAtual.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        SpriteRenderer spriteRenderer = letra.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            corOriginal = spriteRenderer.color;
            spriteRenderer.color = Color.gray;
        }
    }

    public void LimparReferencia()
    {
        if (letraAtual != null)
        {
            Collider2D col = letraAtual.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }

            SpriteRenderer spriteRenderer = letraAtual.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = corOriginal;
            }
            letraAtual = null;
        }
    }

    public void SetLetraEsperada(string letra)
    {
        letraEsperada = letra.ToUpper();
    }

    public bool LetraColocada()
    {
        return letraAtual != null;
    }

    public bool VerificarLetra()
    {
        return letraAtual != null && letraAtual.name.ToUpper() == letraEsperada;
    }

    public string GetLetraAtual()
    {
        return letraAtual != null ? letraAtual.name : null;
    }
}
