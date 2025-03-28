using UnityEngine;

public class MarisaUltInfo : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 10;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 direction)
    {
        GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Apply dmg
        RedYokai redYokai= collision.GetComponent<RedYokai>();
        if (redYokai != null)
        {
            redYokai.stateMachine.ChangeState(redYokai.hurtState);
        }
    }
}
