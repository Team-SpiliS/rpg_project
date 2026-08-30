public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    protected EnemyBase enemy;
    public bool IsLocked { get; protected set; }


    public void LockState() => IsLocked = true;
    public void UnlockState() => IsLocked = false;
    public EnemyStateMachine(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize(IEnemyState initialState)
    {
        UnlockState();
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (IsLocked || newState == null || enemy.IsDead) return;
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void ForceChangeState(IEnemyState newState)
    {
        if (newState == null || enemy.IsDead) return;
        UnlockState();
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
