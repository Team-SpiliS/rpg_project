using UnityEngine;

public class EnemyIdleState : AbstractEnemyState
{
    public EnemyIdleState(EnemyBase enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.animator.CrossFade(enemy.animData.idle, 0.2f);
        if (enemy.agent.isOnNavMesh) enemy.agent.ResetPath();
    }

    public override void LogicUpdate()
    {
        bool isPeaceful = enemy.gameSettings.IsPeacefulMode;

        if (enemy is BossEnemy)
        {
            if (isPeaceful)
            {
                if (enemy.wasHitByPlayer)
                    enemy.StateMachine.ChangeState(new BossChaseState(enemy));
                return; 
            }
        }
        else 
        {
            if (isPeaceful)
            {
                if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold)
                {
                    enemy.StateMachine.ChangeState(new EnemyFleeState(enemy));
                }
                return; 
            }
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist < enemy.detectionRange || enemy.wasHitByPlayer)
        {
            if (enemy is BossEnemy)
                enemy.StateMachine.ChangeState(new BossChaseState(enemy));
            else
                enemy.StateMachine.ChangeState(new EnemyChaseState(enemy));
        }
    }
}