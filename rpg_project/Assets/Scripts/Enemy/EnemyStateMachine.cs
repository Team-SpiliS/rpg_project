public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    protected EnemyBase enemy;

    public EnemyStateMachine(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize(IEnemyState initialState)
    {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public virtual IEnemyState CreateAttackState() => null;
}