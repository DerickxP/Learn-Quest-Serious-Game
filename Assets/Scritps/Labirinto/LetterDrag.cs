using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
        rb.simulated = false;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.simulated = true; 
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
