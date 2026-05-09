public class BasicEnemy : EnemyBase
{
    protected override void Awake()
    {
        StateMachine = new EnemyStateMachine(this, () => new EnemyAttackState(this));
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(new EnemyIdleState(this));
    }

    public override AbstractEnemyState CreateAttackState()
        => (AbstractEnemyState)StateMachine.CreateAttackState();
}