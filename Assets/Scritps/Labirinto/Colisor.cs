using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisor : MonoBehaviour
{
    [Header("Colisor Manager")]
    private Rigidbody2D rb;
    private Vector2 lastValidPosition;
    public string blockingTag = "Blocking";

    [Header("Reference")]
    private ManagerLabirinto ML;
    private Generaltimer timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastValidPosition = rb.position;

        ML = FindObjectOfType<ManagerLabirinto>();
        if (ML == null)
        {
            Debug.LogError("O script ManagerLabirinto não foi encontrado na cena.");
        }

        timer = FindObjectOfType<Generaltimer>();
        if (timer == null)
        {
            Debug.LogError("O script Generaltimer não foi encontrado na cena.");
        }
    }

    void FixedUpdate()
    {
        lastValidPosition = rb.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(blockingTag))
        {
            Debug.Log(collision.gameObject.name);
            rb.velocity = Vector2.zero;
            rb.MovePosition(lastValidPosition);
        }

        if (collision.gameObject.CompareTag("VictoryCondition"))
        {
            Debug.Log("Colidiu com a Condição de Vitória");
            ML.TeladeVitoria.SetActive(true);
            timer.PauseTimer();
        }
    }
}
