using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float verticalLiftHeight = 2f;
    public float randomHorizontalRange = 1f;

    [Header("Animation References")]
    public Animator bombAnimator;

    [Header("Physics Settings")]
    public float initialVerticalVelocity = 5f;
    public float gravityMultiplier = 1f;

    private Rigidbody2D rb;
    private bool isDropped = false;
    private bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure animator is set
        if (bombAnimator == null)
        {
            bombAnimator = GetComponent<Animator>();
        }

        // Disable gravity initially
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
    }

    public void Drop()
    {
        if (isDropped) return;
        isDropped = true;

        // Play idle animation
        if (bombAnimator != null)
        {
            bombAnimator.SetTrigger("Idle");
        }

        // Calculate random horizontal movement
        float randomX = Random.Range(-randomHorizontalRange, randomHorizontalRange);

        // Apply initial vertical and horizontal velocity
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(randomX, initialVerticalVelocity);
            rb.gravityScale = gravityMultiplier;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ensure we only explode once and on ground collision
        if (!hasExploded && IsGroundCollision(collision))
        {
            Explode();
        }
    }

    private bool IsGroundCollision(Collision2D collision)
    {
        // You might want to adjust this based on your ground layer or tag
        return collision.gameObject.CompareTag("Ground");
    }

    private void Explode()
    {
        hasExploded = true;

        // Stop physics
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Trigger explosion animation
        if (bombAnimator != null)
        {
            bombAnimator.SetTrigger("Explode");
        }

        // Optional: Destroy the bomb after explosion animation
        Destroy(gameObject, GetExplosionAnimationLength());
    }

    private float GetExplosionAnimationLength()
    {
        // If you have an explosion animation, return its length
        // Otherwise, return a default time
        return bombAnimator != null
            ? bombAnimator.GetCurrentAnimatorStateInfo(0).length
            : 1f;
    }
}