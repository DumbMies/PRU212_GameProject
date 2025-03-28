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
        RedYokai redYokai = collision.GetComponent<RedYokai>();
        if (redYokai != null)
        {
            redYokai.stateMachine.ChangeState(redYokai.hurtState);
        }

        Destroy(gameObject);
    }
}
