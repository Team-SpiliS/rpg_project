using UnityEngine;

public class EnemyChaseState : AbstractEnemyState
{
    public EnemyChaseState(EnemyBase enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.animator.CrossFade(enemy.animData.chase, 0.2f);
    }

    public override void LogicUpdate()
    {
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold &&
        enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(enemy.CreateFleeState());
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist <= enemy.attackRange)
        {
            enemy.StateMachine.ChangeState(enemy.CreateAttackState());
            return;
        }

        if (dist > enemy.detectionRange && !enemy.wasHitByPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.CreateIdleState());
            return;
        }

        if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(enemy.player.position);
    }
}

public class RangedChaseState : AbstractEnemyState
{
    private readonly RangedEnemy rangedEnemy;

    public RangedChaseState(RangedEnemy enemy) : base(enemy)
    {
        rangedEnemy = enemy;
    }

    public override void Enter()
    {
        enemy.animator.CrossFade(enemy.animData.chase, 0.2f);
    }

    public override void LogicUpdate()
    {
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold &&
        enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(enemy.CreateFleeState());
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist < rangedEnemy.weaponConfig.retreatDistance)
        {
            Vector3 retreatDir = (enemy.transform.position - enemy.player.position).normalized;
            Vector3 retreatPos = enemy.transform.position + retreatDir * 5f;
            if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(retreatPos);
            return;
        }

        if (dist <= enemy.attackRange)
        {
            enemy.StateMachine.ChangeState(enemy.CreateAttackState());
            return;
        }

        if (dist > enemy.detectionRange && !enemy.wasHitByPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.CreateIdleState());
            return;
        }

        if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(enemy.player.position);
    }
}
