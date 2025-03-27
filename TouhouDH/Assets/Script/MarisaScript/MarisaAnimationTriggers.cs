using UnityEngine;

public class MarisaAnimationTriggers : MonoBehaviour
{
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
            WingedYoukaiAI enemy1 = hit.GetComponentInParent<WingedYoukaiAI>();
            ThunderYoukai enemy2 = hit.GetComponentInParent<ThunderYoukai>();
            if (enemy1 != null && hit == enemy1.hitbox)
            {
                enemy1.TakeDamage();
            }
            if (enemy2 != null)
            {
                enemy2.TakeDamage();
            }
        }
    }
}
