using System.Collections;
using UnityEngine;

public class ThunderYoukai : Youkai
{
    public GameObject lightningWarn;
    public GameObject lightning;
    public GameObject lightningExplosion;
    private Marisa player;

    #region States
    public ThunderYoukaiIdleState idleState { get; private set; }
    public ThunderYoukaiMoveState moveState { get; private set; }
    public ThunderYoukaiBattleState battleState { get; private set; }
    public ThunderYoukaiAttackState attackState { get; private set; }
    public ThunderYoukaiHurtState hurtState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ThunderYoukaiIdleState(this, stateMachine, "Idle", this);
        moveState = new ThunderYoukaiMoveState(this, stateMachine, "Move", this);
        battleState = new ThunderYoukaiBattleState(this, stateMachine, "Move", this);

        attackState = new ThunderYoukaiAttackState(this, stateMachine, "Attack", this);
        hurtState = new ThunderYoukaiHurtState(this, stateMachine, "Hurt", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        player = FindObjectOfType<Marisa>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override RaycastHit2D IsPlayerDetected() => Physics2D.BoxCast(transform.position, new Vector2(5, 2f), 0, Vector2.right * facingDirection, 1, whatIsPlayer);

    public void TakeDamage()
    {
        stateMachine.ChangeState(hurtState);
    }

    #region lightning strike
    public void TriggerLightningStrike()
    {
        if (player != null)
        {
            Vector3 groundPosition = new Vector3(player.transform.position.x, GetGroundYPosition(player.transform.position), player.transform.position.z);
            GameObject warning = Instantiate(lightningWarn, groundPosition, Quaternion.identity);
            StartCoroutine(StrikeLightningAfterDelay(groundPosition, warning));
        }
    }

    private float GetGroundYPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return hit.point.y;
        }
        return position.y;
    }

    private IEnumerator StrikeLightningAfterDelay(Vector3 position, GameObject warning)
    {
        yield return new WaitForSeconds(1f); // Delay before the lightning strike
        Destroy(warning);
        GameObject lightningInstance = Instantiate(lightning, position, Quaternion.identity);
        yield return new WaitForSeconds(0.06f);
        GameObject lightningExplosionInstance = Instantiate(lightningExplosion, position, Quaternion.identity);
        yield return new WaitForSeconds(0.25f); // Delay before destroying the objects
        Destroy(lightningInstance);
        Destroy(lightningExplosionInstance);
    }
    #endregion
}
