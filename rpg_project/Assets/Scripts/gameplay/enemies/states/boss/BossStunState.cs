using UnityEngine;
using System.Collections;

public class BossStunState : BossState
{
    private Coroutine _stunRoutine;

    public BossStunState(BossEnemy boss) : base(boss) { }

    public override void Enter()
    {
        base.Enter();

        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        enemy.StateMachine.LockState();

        _stunRoutine = enemy.StartCoroutine(StunRoutine());
    }

    public override void Exit()
    {
        if (_stunRoutine != null)
        {
            enemy.StopCoroutine(_stunRoutine);
            _stunRoutine = null;
        }

        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
        boss.ResetStunMeter();
        base.Exit();
    }

    public override void LogicUpdate() { }

    private IEnumerator StunRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.stun, 0.1f);
        yield return new WaitForSeconds(3.0f);
        if (boss.IsDead) yield break;

        enemy.StateMachine.UnlockState();

        _stunRoutine = null;
        enemy.StateMachine.ChangeState(boss.CreateChaseState());
    }

    protected override void HandleStunTriggered()
    {
    }
}
