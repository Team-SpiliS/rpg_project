using UnityEngine;
using System.Collections;

public class BossStunState : AbstractEnemyState
{
    private BossEnemy _boss;

    public BossStunState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        enemy.StartCoroutine(StunRoutine());
    }

    public override void Exit()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    private IEnumerator StunRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.stun, 0.1f);
        Debug.Log("аняя нцксьем");

        yield return new WaitForSeconds(3.0f); 

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }
}