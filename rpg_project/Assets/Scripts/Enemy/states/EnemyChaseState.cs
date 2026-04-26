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
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold)
        {
            enemy.StateMachine.ChangeState(new EnemyFleeState(enemy));
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (enemy is RangedEnemy mage && dist < mage.retreatDistance)
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
            enemy.StateMachine.ChangeState(new EnemyIdleState(enemy));
            return;
        }

        if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(enemy.player.position);
    }
}