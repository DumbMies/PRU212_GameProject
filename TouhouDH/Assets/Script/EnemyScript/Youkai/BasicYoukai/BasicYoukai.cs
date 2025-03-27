using UnityEngine;

public class BasicYoukai : Youkai
{

    #region States
    public BasicYoukaiIdleState idleState { get; private set; }
    public BasicYoukaiMoveState moveState { get; private set; }
    public BasicYoukaiBattleState battleState { get; private set; }
    public BasicYoukaiAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new BasicYoukaiIdleState(this, stateMachine, "Idle", this);
        moveState = new BasicYoukaiMoveState(this, stateMachine, "Move", this);
        battleState = new BasicYoukaiBattleState(this, stateMachine, "Move", this);

        attackState = new BasicYoukaiAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
