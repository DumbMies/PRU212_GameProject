using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Marisa : Entity
{
    public Boss2 boss2 => GetComponent<Boss2>();
    public bool isBusy { get; private set; }
    public bool isHurt { get; private set; }
    public bool canMove { get; private set; } = true;
    [SerializeField] private Transform Trails;
    private Transform WhiteTrail;
    private Transform OrangeTrail;

    [Header("Attack info")]
    public Vector2[] attackMovement;

    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float DashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }

    #region States
    public MarisaStateMachine stateMachine { get; private set; }
    public MarisaIdleState idleState { get; private set; }
    public MarisaMoveState moveState { get; private set; }
    public MarisaAirState airState { get; private set; }
    public MarisaJumpState jumpState { get; private set; }
    public MarisaDashState dashState { get; private set; }
    public MarisaHurtState hurtState { get; private set; }

    public MarisaPrimaryAttackState primaryAttack { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new MarisaStateMachine();

        idleState = new MarisaIdleState(stateMachine, this, "Idle");
        moveState = new MarisaMoveState(stateMachine, this, "Move");
        airState = new MarisaAirState(stateMachine, this, "Jump");
        jumpState = new MarisaJumpState(stateMachine, this, "Jump");
        dashState = new MarisaDashState(stateMachine, this, "Dash");
        hurtState = new MarisaHurtState(stateMachine, this, "Hurt");

        primaryAttack = new MarisaPrimaryAttackState(stateMachine, this, "PrimaryAttack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        Trails = GameObject.Find("Trails").transform;
        WhiteTrail = Trails.Find("White").transform;
        OrangeTrail = Trails.Find("Orange").transform;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();
    }

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;
        yield return new WaitForSeconds(_second);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        DashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && DashUsageTimer < 0 && canMove)
        {
            DashTrail();

            DashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SkillShots"), true);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);


            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            stateMachine.ChangeState(dashState);
            EnableCollisionAfterDuration(250);
        }
    }
    private void DamageTestInput()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            boss2.TakeDamage();
        }
    }

    async void EnableCollisionAfterDuration(int Duration)
    {
        await Task.Delay(Duration);

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SkillShots"), false);
    }
    async void DashTrail()
    {
        //Debug.Log(Trails.childCount);
        //for (int i = 0; i < Trails.childCount; i++)
        //{
        //    Trails.GetChild(i).transform.GetComponent<TrailRenderer>().enabled = true;
        //}
        //await Task.Delay(250);
        //for (int i = 0; i < Trails.childCount; i++)
        //{
        //    Trails.GetChild(i).transform.GetComponent<TrailRenderer>().enabled = false;
        //}
        WhiteTrail.GetComponent<TrailRenderer>().emitting = true;
        OrangeTrail.GetComponent<TrailRenderer>().emitting = true;

        await Task.Delay(250);
        WhiteTrail.GetComponent<TrailRenderer>().emitting = false;
        OrangeTrail.GetComponent<TrailRenderer>().emitting = false;
    }
    public async void TriggerInvincibleFrame()
    {
        isHurt = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SkillShots"), true);
        canMove = false;
        await Task.Delay(1000);
        ExitInvincibleFrame();
    }
    private void ExitInvincibleFrame()
    {
        isHurt = false;
        canMove = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SkillShots"), false);
    }
}
