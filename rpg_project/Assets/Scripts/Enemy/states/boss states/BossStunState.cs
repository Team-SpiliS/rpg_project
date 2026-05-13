using UnityEngine;
using System.Collections;

public class BossStunState : AbstractEnemyState
{
    private BossEnemy _boss;

    public BossStunState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void Enter()
    {
        Debug.Log('3');

        _boss.OnPhaseChanged += HandlePhaseChange;


        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        enemy.StateMachine.LockState();

        enemy.StartCoroutine(StunRoutine());
    }

    public override void Exit()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
        _boss.OnPhaseChanged -= HandlePhaseChange;


    }

    public override void LogicUpdate() { }

    private IEnumerator StunRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.stun, 0.1f);
        yield return new WaitForSeconds(3.0f);
        enemy.StateMachine.UnlockState();

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }

    public void HandlePhaseChange()
    {
        enemy.StateMachine.UnlockState();

        _boss.StateMachine.ChangeState(new BossTauntState(enemy));
        return;
    }
}