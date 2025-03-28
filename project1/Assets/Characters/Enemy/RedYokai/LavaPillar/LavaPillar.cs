using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class LavaPillar : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float knockbackForce = 5f;

    private BoxCollider2D boxCollider;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.Play("Burst");   
    }
    public void ActivateHitbox()
    {
        boxCollider.enabled = true;
    }
    public void DestroyPillar()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Marisa marisa = collision.gameObject.GetComponent<Marisa>();

            if (!marisa.stateMachine.currentState.Equals(marisa.hurtState))
            {
                Animator playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();
                playerAnimator.SetBool("Hurt", true);
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Jump", false);
                playerAnimator.SetBool("Move", false);

                collision.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                PlayerTakeDamage(playerAnimator, collision.gameObject);
                Debug.Log("CodeReachedHere");
                marisa.stateMachine.ChangeState(marisa.hurtState);

                if (collision.transform.position.x < transform.position.x)
                {
                    collision.gameObject.GetComponent<Marisa>().rb.linearVelocity = new Vector2(-knockbackForce, knockbackForce*1.5f);
                    if (marisa.facingDirection == -1)
                    {
                        marisa.Flip();
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<Marisa>().rb.linearVelocity = new Vector2(knockbackForce, knockbackForce);
                    if (marisa.facingDirection == 1)
                    {
                        marisa.Flip();
                    }
                }

                //Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                //Vector2 knockbackDirection = (collision.gameObject.transform.position - transform.position).normalized;
                //playerRb.linearVelocity = new Vector2(knockbackDirection.x * knockbackForce, knockbackForce);
            }
        }
    }

    async void PlayerTakeDamage(Animator playerAnimator, GameObject playerSprite)
    {
        ColorTween(playerSprite.GetComponentInChildren<SpriteRenderer>(), Color.red, Color.white, 1f);

        await Task.Delay(150);
        
        //playerSprite.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, 0.2f);
        playerAnimator.SetBool("Hurt", false);
    }

    async void ColorTween(SpriteRenderer sprite ,Color StartColor, Color EndColor, float Duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / Duration;
            Color newColor = Color.Lerp(StartColor, EndColor, t);
            sprite.color = newColor;
            await Task.Yield();
        }
    }
}