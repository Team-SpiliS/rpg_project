using UnityEngine;

public class BossIdleState : AbstractEnemyState
{
    private BossEnemy _boss;

    public BossIdleState(EnemyBase enemy) : base(enemy)
    {
        _boss = enemy as BossEnemy;
    }

    public override void Enter()
    {
        enemy.animator.CrossFade(enemy.animData.idle, 0.2f);
        if (enemy.agent.isOnNavMesh) enemy.agent.ResetPath();

        enemy.wasHitByPlayer = false;
    }

    public override void LogicUpdate()
    {
        bool isPeaceful = enemy.gameSettings.IsPeacefulMode;
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (isPeaceful)
        {
            if (enemy.wasHitByPlayer)
            {
                enemy.StateMachine.ChangeState(new BossChaseState(enemy));
            }
        }
        else
        {
            if (enemy.wasHitByPlayer || dist < enemy.detectionRange)
            {
                enemy.StateMachine.ChangeState(new BossChaseState(enemy));
            }
        }
    }
}