using UnityEngine;

public class MarisaLongAttackInfo : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 2;
    public float speed = 15f;

    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Apply damage
        WingedYoukaiAI wingedYoukai = collision.GetComponent<WingedYoukaiAI>();
        ThunderYoukai thunderYoukai = collision.GetComponent<ThunderYoukai>();
        if (wingedYoukai != null)
        {
            wingedYoukai.TakeDamage();
        }
        if (thunderYoukai != null)
        {
            thunderYoukai.TakeDamage();
        }

        Destroy(gameObject);
    }
}
