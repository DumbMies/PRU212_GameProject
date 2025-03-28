using UnityEngine;

public class RedYokaiDieState : RedYokaiState
{
    private float fadeDuration = 2f; // Time in seconds to fade out
    private float fadeTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    public RedYokaiDieState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName)
        : base(_stateMachine, _redYokai, _animBoolName)
    {
        // Find the SpriteRenderer inside the Animator GameObject
        spriteRenderer = _redYokai.transform.Find("AnimatorGameObjectName")?.GetComponent<SpriteRenderer>();
    }

    public override void Enter()
    {
        base.Enter();
        redYokai.SetZeroVelocity(); // Stop movement
        fadeTimer = 0f; // Reset fade timer
        if (spriteRenderer != null)
            spriteColor = spriteRenderer.color; // Store the original color
        redYokai.hitbox = null;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            redYokai.anim.speed = 0;
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration); // Gradually decrease alpha

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            }

            if (fadeTimer >= fadeDuration)
            {
                GameObject.Destroy(redYokai.gameObject); // Destroy after fade out
            }
        }
    }
}
