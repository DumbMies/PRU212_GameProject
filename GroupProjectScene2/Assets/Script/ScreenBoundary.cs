using UnityEngine;

public class ScreenBoundary : MonoBehaviour
{
    private Camera mainCamera;
    private float minX, maxX;
    private float objectWidth;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            CalculateBoundaries();
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            objectWidth = collider.bounds.extents.x;
        }
        else
        {
            objectWidth = 0.5f; 
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX + objectWidth, maxX - objectWidth);
        transform.position = pos;
    }

    void CalculateBoundaries()
    {
        float vertExtent = mainCamera.orthographicSize;
        float horizExtent = vertExtent * Screen.width / Screen.height;

        minX = mainCamera.transform.position.x - horizExtent;
        maxX = mainCamera.transform.position.x + horizExtent;
    }
}