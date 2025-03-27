using UnityEngine;
public class WingedYoukaiBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public float damage = 1;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Apply dmg
            Marisa player = collision.GetComponent<Marisa>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
            }

            Destroy(gameObject);
        }
    }
}
