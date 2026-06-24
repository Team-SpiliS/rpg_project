using UnityEngine;

public class RangedEnemy : EnemyBase
{

    protected override void Awake()
    {
        StateMachine = new EnemyStateMachine(this);
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(CreateIdleState());
    }

    public override IEnemyState CreateChaseState()
    {
        return new RangedChaseState(this);
    }

    public override IEnemyState CreateAttackState()
    {
        return new EnemyRangedAttackState(this);
    }
}
