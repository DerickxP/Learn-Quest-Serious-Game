using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    [SerializeField] private bool estaSendoArrastado = false;
    [SerializeField] private Vector3 posicaoInicial;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            switch (toque.phase)
            {
                case TouchPhase.Began:
                    Vector3 posicaoToque = Camera.main.ScreenToWorldPoint(toque.position);
                    RaycastHit2D hit = Physics2D.Raycast(posicaoToque, Vector2.zero);

                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        estaSendoArrastado = true;
                        posicaoInicial = transform.position - posicaoToque;
                    }
                    break;

                case TouchPhase.Moved:
                    if (estaSendoArrastado)
                    {
                        Vector3 novaPosicao = Camera.main.ScreenToWorldPoint(toque.position) + posicaoInicial;
                        rb2d.MovePosition(new Vector2(novaPosicao.x, novaPosicao.y));
                    }
                    break;

                case TouchPhase.Ended:
                    estaSendoArrastado = false;
                    break;
            }
        }
    }
}

