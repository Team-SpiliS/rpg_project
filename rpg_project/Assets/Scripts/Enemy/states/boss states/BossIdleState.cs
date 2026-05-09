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
    }

    public override void LogicUpdate()
    {
        if (enemy.wasHitByPlayer || Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRange)
        {
            enemy.StateMachine.ChangeState(new BossChaseState(enemy));
        }
    }
}