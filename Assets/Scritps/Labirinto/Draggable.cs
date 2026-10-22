using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private Rigidbody2D rb;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Nenhum Rigidbody2D encontrado no objeto.");
        }
    }

    void OnMouseDown()
    {
        if (rb != null)
        {
            // Calcula o offset entre a posição do objeto e a posição do mouse no mundo
            offset = gameObject.transform.position - GetMouseWorldPos();
            isDragging = true;
        }
    }

    void OnMouseUp()
    {
        if (rb != null)
        {
            isDragging = false;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && rb != null)
        {
            // Atualiza a posição do Rigidbody2D para seguir o mouse
            rb.MovePosition(GetMouseWorldPos() + offset);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Captura a posição do mouse na tela e converte para posição no mundo
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;

        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
