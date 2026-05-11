public abstract class AbstractEnemyState : IEnemyState
{
    protected EnemyBase enemy;

    public AbstractEnemyState(EnemyBase enemy)
    {
        this.enemy = enemy;
    }

    protected bool CheckGlobalTransitions()
    {
        if (enemy is BossEnemy boss)
        {
            if (!boss.isPhaseTwo && boss.myHealth.GetCurrentHealth() <= boss.myHealth.GetMaxHealth() * 0.5f)
            {
                boss.StateMachine.ChangeState(new BossTauntState(enemy));
                return true;
            }
            if (boss.damageTakenRecently >= boss.stunDamageThreshold)
            {
                boss.ResetStunMeter();
                boss.StateMachine.ChangeState(new BossStunState(enemy));
                return true;
            }
        }
        return false;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}