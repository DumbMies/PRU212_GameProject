using UnityEngine;

public class BossHover : MonoBehaviour
{
    public float hoverSpeed = 2f;
    public float hoverHeight = 0.5f;
    private float originalY;

    void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight, transform.position.z);
    }
}
