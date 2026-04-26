using UnityEngine;
using System.Collections;

public class BossTauntState : AbstractEnemyState
{
    private BossEnemy _boss;

    public BossTauntState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;

        if (enemy is BossEnemy boss) boss.isInvulnerable = true;

        enemy.StartCoroutine(TauntRoutine());
    }

    public override void Exit()
    {
        if (enemy is BossEnemy boss) boss.isInvulnerable = false;

        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    private IEnumerator TauntRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.taunt, 0.2f);
        Debug.Log("ÁÎÑÑ Â ÔÀÇÅ 2");

        yield return new WaitForSeconds(2.5f);

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }
}