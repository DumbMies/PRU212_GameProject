using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RedYokaiGroundSlamState : RedYokaiState
{
    RaycastHit2D GroundBeneath;
    bool PillarsSpawned;
    public RedYokaiGroundSlamState(RedYokaiStateMachine _stateMachine, RedYokai _redYokai, string _animBoolName) : base(_stateMachine, _redYokai, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        PillarsSpawned = false;
        base.Enter();
        redYokai.rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(redYokai.moveState);
            triggerCalled = false;
        }
        if (redYokai.distanceToGround <= 0.5f && PillarsSpawned == false)
        {
            redYokai.anim.speed = 1;
            rb.gravityScale = 1;
            DetectAndSpawnLavaPillars();
            Cooldown();
            redYokai.canAttack = false;
            PillarsSpawned = true;
        }
    }

    //async void GroundSlam()
    //{
    //    await Task.Delay(250);
    //    rb.gravityScale = 20;
    //    await Task.Delay(500);

    //    rb.gravityScale = 1;

    //    redYokai.AttackCooldownStart();
    //}

    public void StartCharging()
    {
        Debug.Log("Start Charging");
        redYokai.anim.speed = 0.8f;
        rb.gravityScale = -0.5f;
    }

    public void SlamDown()
    {
        Debug.Log("Slam Down");
        redYokai.anim.speed = 0;
        rb.gravityScale = 20f;
    }

    public void DetectAndSpawnLavaPillars()
    {
        GroundBeneath = redYokai.FindGroundByDistance(Mathf.Infinity);

        if (GroundBeneath.collider == null) return;

        float yLevel = GroundBeneath.point.y;
        float startX = GroundBeneath.point.x;

        SpawnLavaPillarsWithDelay(startX, yLevel);
    }

    async void SpawnLavaPillarsWithDelay(float startX, float yLevel)
    {
        float distanceBetweenPillar = 1.5f;
        float timeBetweenPillar = 0.15f;

        float x = startX;
        int faceDirectionWhenSkillCast = redYokai.facingDirection;

        while (true)
        {
            GameObject newPillar = Entity.Instantiate(redYokai.lavaPillarPrefab, new Vector3(x, yLevel, 0), Quaternion.identity);
            newPillar.GetComponent<LavaPillar>().enabled = true;
            newPillar.GetComponent<BoxCollider2D>().enabled = true;

            RaycastHit2D groundCheck = Physics2D.BoxCast(newPillar.transform.position, new Vector2(0.4f, 0.1f), 0, Vector2.down, 0.1f, redYokai.GetGround());
            RaycastHit2D wallCheck = Physics2D.Raycast(new Vector2(newPillar.transform.position.x, newPillar.transform.position.y + 0.5f), 
                Vector2.right * faceDirectionWhenSkillCast, 0.5f);

            if (groundCheck.collider == null)
            {
                Entity.Destroy(newPillar);
                break;
            }
            if (faceDirectionWhenSkillCast == 1)
            {
                x += distanceBetweenPillar;
            }
            else
            {
                x -= distanceBetweenPillar;
            }
            await Task.Delay(TimeSpan.FromSeconds(timeBetweenPillar));
        }
    }
}
