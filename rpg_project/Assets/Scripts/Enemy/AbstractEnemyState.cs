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