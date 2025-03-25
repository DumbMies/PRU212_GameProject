using UnityEngine;

public class ThunderYoukaiLightning : MonoBehaviour
{
    public float damage = 1;

    void Start()
    {
        
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
        }
    }
}
