using UnityEngine;
using System.Collections;

public class BossTauntState : AbstractEnemyState
{
    private BossEnemy _boss;

    public BossTauntState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        _boss.isInvulnerable = true;
        enemy.StateMachine.LockState();

        enemy.StartCoroutine(TauntRoutine());

    }

    public override void Exit()
    {
        _boss.isInvulnerable = false;
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    public override void LogicUpdate() { }

    private IEnumerator TauntRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.taunt, 0.2f);
        yield return new WaitForSeconds(2.5f);

        _boss.isPhaseTwo = true;
        enemy.StateMachine.UnlockState();

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }
}