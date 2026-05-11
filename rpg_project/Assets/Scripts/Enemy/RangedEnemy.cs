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
        StateMachine.Initialize(new EnemyIdleState(this));
    }

    public override AbstractEnemyState CreateAttackState()
    {
        return new EnemyRangedAttackState(this);
    }
}