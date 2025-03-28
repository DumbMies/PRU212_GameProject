using UnityEngine;

public class Boss2Attack3LaunchState : Boss2State
{
    private bool spearSpawned;
    private Transform currentCheckpoint;

    public Boss2Attack3LaunchState(Boss2StateMachine stateMachine, Boss2 boss2, string animBoolName)
        : base(stateMachine, boss2, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        boss2.capsuleCollider2D.isTrigger = true;
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] hitColliders = Physics2D.OverlapCapsuleAll(
            boss2.capsuleCollider2D.bounds.center,
            boss2.capsuleCollider2D.bounds.size,
            CapsuleDirection2D.Vertical,
            0f,
            LayerMask.GetMask("Player") 
        );

        // Check if any of the overlapping colliders are the player
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Marisa marisa = hitCollider.GetComponent<Marisa>();
                marisa.TakeDamage(1);
                break;
            }
        }
        if (boss2.IsWallDetected())
        {
            boss2.Flip();
            stateMachine.ChangeState(boss2.knockedState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss2.capsuleCollider2D.isTrigger = false;
    }
}