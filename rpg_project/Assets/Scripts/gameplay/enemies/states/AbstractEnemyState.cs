public abstract class AbstractEnemyState : IEnemyState
{
    protected EnemyBase enemy;

    public AbstractEnemyState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }


    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}

public abstract class BossState : AbstractEnemyState
{
    protected readonly BossEnemy boss;

    protected BossState(BossEnemy boss) : base(boss)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        boss.OnPhaseChanged += HandlePhaseChange;
        boss.OnStunTriggered += HandleStunTriggered;
    }

    public override void Exit()
    {
        boss.OnPhaseChanged -= HandlePhaseChange;
        boss.OnStunTriggered -= HandleStunTriggered;
    }

    protected virtual void HandlePhaseChange()
    {
        boss.StateMachine.ForceChangeState(boss.CreateTauntState());
    }

    protected virtual void HandleStunTriggered()
    {
        boss.StateMachine.ForceChangeState(boss.CreateStunState());
    }
}
