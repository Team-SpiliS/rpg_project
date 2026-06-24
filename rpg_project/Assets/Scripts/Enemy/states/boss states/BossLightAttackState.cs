using UnityEngine;
using System.Collections;

public class BossLightAttackState : BossState
{
    private Coroutine _attackRoutine;

    public BossLightAttackState(BossEnemy boss) : base(boss) { }


    public override void Enter()
    {
        base.Enter();
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        _attackRoutine = enemy.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        if (_attackRoutine != null) enemy.StopCoroutine(_attackRoutine);
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
        enemy.animator.speed = 1f;
        base.Exit();
    }

    private IEnumerator AttackRoutine()
    {
        enemy.RotateTowardsPlayer();

        enemy.animator.speed = boss.isPhaseTwo ? 1.5f : 1f;
        enemy.animator.CrossFade(enemy.animData.attackTrigger, 0.1f);

        float delay = boss.isPhaseTwo ? 0.4f / 1.5f : 0.4f;
        yield return new WaitForSeconds(delay);
        if (boss.IsDead) yield break;

        boss.PlayMeleeHitEffects();

        if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.attackRange + 0.5f)
        {
            enemy.playerHealth?.TakeDamage(15, DamageType.Physical);
        }

        yield return new WaitForSeconds(0.6f);
        if (boss.IsDead) yield break;

        _attackRoutine = null;
        enemy.StateMachine.ChangeState(boss.CreateChaseState());
    }
}
