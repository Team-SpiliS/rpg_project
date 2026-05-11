using System;

public class EnemyStateMachine
{
    public IEnemyState CurrentState { get; private set; }
    protected EnemyBase enemy;
    private Func<IEnemyState> _attackFactory;

    public EnemyStateMachine(EnemyBase enemy, Func<IEnemyState> attackFactory = null)
    {
        this.enemy = enemy;
        _attackFactory = attackFactory;
    }

    public virtual IEnemyState CreateAttackState()
    {
        return _attackFactory?.Invoke();
    }

    public void Initialize(IEnemyState initialState)
    {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(IEnemyState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}