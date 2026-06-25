public class BasicEnemy : EnemyBase
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

    public override IEnemyState CreateAttackState() => new EnemyAttackState(this);
}
