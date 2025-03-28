using UnityEngine;

public class MarisaAnimationTriggers : MonoBehaviour
{
    public int hitCount = 0;
    private Marisa player => GetComponentInParent<Marisa>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var hit in colliders)
        {
            RedYokai redYokai = hit.GetComponentInParent<RedYokai>();
            Boss2 boss2 = hit.GetComponentInParent<Boss2>();
            if (redYokai != null && hit == redYokai.hitbox)
            {
                redYokai.rb.linearVelocity = new Vector2(player.facingDirection * 5f, 5f);
                redYokai.TakeDamage(1);
            }
            if (boss2 != null && hit == boss2.hitbox)
            {
                if (hitCount < 3)
                {
                    hitCount += 1;
                    print("Code reached here");
                    boss2.stateMachine.ChangeState(boss2.knockedState);
                    boss2.TakeDamage(1);
                }
            }
        }
    }
}
