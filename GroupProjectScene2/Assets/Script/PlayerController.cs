using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Vector3 originalScale;

    private bool isTrappedAtEdge = false;
    public float edgePushForce = 10f;
    public float trappedCheckDistance = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.mass = 1.5f; 

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rb.linearVelocity.y) > jumpForce * 1.5f)
        {
            Vector2 clampedVelocity = rb.linearVelocity;
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -jumpForce * 1.5f, jumpForce * 1.5f);
            rb.linearVelocity = clampedVelocity;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        CheckIfTrapped();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        Vector2 targetVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime * 10f);

        if (move > 0)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (move < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        animator.SetBool("isRunning", move != 0);
    }


    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    public void Attack()
    {
        animator.SetTrigger("isFighting");
    }

    void CheckIfTrapped()
    {
        Camera mainCamera = Camera.main;
        float horizExtent = mainCamera.orthographicSize * Screen.width / Screen.height;
        float minX = mainCamera.transform.position.x - horizExtent + 0.5f;
        float maxX = mainCamera.transform.position.x + horizExtent - 0.5f;

        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            Transform boss = GameObject.FindGameObjectWithTag("Enemy").transform;
            float distanceToBoss = Vector2.Distance(transform.position, boss.position);

            if ((transform.position.x <= minX + trappedCheckDistance || transform.position.x >= maxX - trappedCheckDistance)
                && distanceToBoss < 2f)
            {
                isTrappedAtEdge = true;
                Vector2 pushDirection = (transform.position - boss.position).normalized;

                Rigidbody2D bossRb = boss.GetComponent<Rigidbody2D>();
                if (bossRb != null)
                {
                    bossRb.AddForce(pushDirection * edgePushForce, ForceMode2D.Impulse);
                }

                rb.AddForce(new Vector2(pushDirection.x * 5f, 0), ForceMode2D.Impulse);
            }
            else
            {
                isTrappedAtEdge = false;
            }
        }
    }
}
