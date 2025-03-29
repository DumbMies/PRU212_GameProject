using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damage = 10;
    public string targetTag = "Player"; 
    public float activeTime = 0.2f; 
    public bool autoDisable = true; 

    private Collider2D hitboxCollider;
    private float disableTime;
    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        if (hitboxCollider == null)
        {
            hitboxCollider = gameObject.AddComponent<BoxCollider2D>();

            if (targetTag == "Player")
            {
                ((BoxCollider2D)hitboxCollider).size = new Vector2(1.5f, 2.5f); 
            }
            else
            {
                ((BoxCollider2D)hitboxCollider).size = new Vector2(1.5f, 1.5f);
            }
        }
        hitboxCollider.isTrigger = true;
        hitboxCollider.enabled = false;
    }

    private void OnEnable()
    {
        if (autoDisable)
        {
            disableTime = Time.time + activeTime;
        }

        // Enable the collider
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }
    }

    private void Update()
    {
        if (autoDisable && Time.time >= disableTime && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"Hitbox hit {other.name} with tag {targetTag}");

            DamageHandler damageHandler = other.GetComponent<DamageHandler>();
            if (damageHandler != null)
            {
                damageHandler.TakeDamage(damage);
            }

            if (targetTag == "Enemy")
            {
                BossAI bossAI = other.GetComponent<BossAI>();
                if (bossAI != null)
                {
                    bossAI.TakeDamage(damage);
                }
            }
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);

        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }

        if (autoDisable)
        {
            disableTime = Time.time + activeTime;
        }
    }

    public void Deactivate()
    {
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }

        gameObject.SetActive(false);
    }
}