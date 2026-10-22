using UnityEngine;

public class MotionControl : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        isDragging = true;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            return hit.point;
        }

        return transform.position;
    }
}
