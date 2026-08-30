using UnityEngine;
using System.Collections;

public class BossTauntState : BossState
{
    private Coroutine _tauntRoutine;

    public BossTauntState(BossEnemy boss) : base(boss) { }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        boss.SetInvulnerable(true);
        enemy.StateMachine.LockState();

        _tauntRoutine = enemy.StartCoroutine(TauntRoutine());

    }

    public override void Exit()
    {
        if (_tauntRoutine != null)
        {
            enemy.StopCoroutine(_tauntRoutine);
            _tauntRoutine = null;
        }

        boss.SetInvulnerable(false);
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    public override void LogicUpdate() { }

    private IEnumerator TauntRoutine()
    {
        enemy.animator.CrossFade(enemy.animData.taunt, 0.2f);
        yield return new WaitForSeconds(2.5f);
        if (boss.IsDead) yield break;

        boss.isPhaseTwo = true;
        enemy.StateMachine.UnlockState();

        _tauntRoutine = null;
        enemy.StateMachine.ChangeState(boss.CreateChaseState());
    }
}
