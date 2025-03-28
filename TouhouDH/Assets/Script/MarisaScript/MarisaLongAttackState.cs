using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarisaLongAttackState : MarisaState
{
    private float _fireDelay = .5f;
    private float _attackCooldown = 2.5f;
    private bool _isAttackOnCooldown = false;

    public MarisaLongAttackState(MarisaStateMachine _stateMachine, Marisa _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {

            base.Enter();
            if (!_isAttackOnCooldown)
            {
                player.StartCoroutine(LongAttackDelay());
                stateTimer = 0.1f;
                player.audioManager.PlaySFX(player.audioManager.longAttackSound, 0.75f);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        
        
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void Shoot()
    {
        if (player.longAttackPrefab != null && player.longAttackPoint != null)
        {
            Vector2 direction = player.isFacingRight ? Vector2.right : Vector2.left;
            GameObject bullet = GameObject.Instantiate(player.longAttackPrefab, player.longAttackPoint.position, Quaternion.identity);
            bullet.GetComponent<MarisaLongAttackInfo>().Initialize(direction);

            // Ensure the bullet is facing the correct direction
            if (!player.isFacingRight)
            {
                bullet.transform.localScale = new Vector3(-bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
            }
        }
        StartLongAttackCooldown();
    }

    private IEnumerator LongAttackDelay()
    {
        yield return new WaitForSeconds(_fireDelay);
        Shoot();
    }

    private void StartLongAttackCooldown()
    {
        _isAttackOnCooldown = true;
        player.StartCoroutine(LongAttackCooldownRoutine());
    }

    private IEnumerator LongAttackCooldownRoutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _isAttackOnCooldown = false;
    }
}
