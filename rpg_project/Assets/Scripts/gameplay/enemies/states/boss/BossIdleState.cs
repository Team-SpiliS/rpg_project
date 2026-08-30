using UnityEngine;

public class BossIdleState : BossState
{
    public BossIdleState(BossEnemy boss) : base(boss) { }

    public override void Enter()
    {
        base.Enter();
        enemy.animator.CrossFade(enemy.animData.idle, 0.2f);
        if (enemy.agent.isOnNavMesh) enemy.agent.ResetPath();

        enemy.wasHitByPlayer = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        bool isPeaceful = enemy.gameSettings.IsPeacefulMode;
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (isPeaceful)
        {
            if (enemy.wasHitByPlayer)
            {
                enemy.StateMachine.ChangeState(boss.CreateChaseState());
            }
        }
        else
        {
            if (enemy.wasHitByPlayer || dist < enemy.detectionRange)
            {
                enemy.StateMachine.ChangeState(boss.CreateChaseState());
            }
        }
    }
}
