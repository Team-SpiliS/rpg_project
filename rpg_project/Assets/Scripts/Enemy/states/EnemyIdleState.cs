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
        if (enemy.gameSettings.IsPeacefulMode) return;

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist < enemy.detectionRange || enemy.wasHitByPlayer)
        {
            enemy.StateMachine.ChangeState(new EnemyChaseState(enemy));
        }
    }
}