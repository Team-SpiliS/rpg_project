using UnityEngine;
using System.Collections;

public class BossLightAttackState : AbstractEnemyState
{
    private BossEnemy _boss;
    private Coroutine _attackRoutine;

    public BossLightAttackState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void LogicUpdate()
    {
        CheckGlobalTransitions();
    }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = true;
        _attackRoutine = enemy.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        if (_attackRoutine != null) enemy.StopCoroutine(_attackRoutine);
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
        enemy.animator.speed = 1f;
    }

    private IEnumerator AttackRoutine()
    {
        enemy.RotateTowardsPlayer();

        enemy.animator.speed = _boss.isPhaseTwo ? 1.5f : 1f;
        enemy.animator.CrossFade(enemy.animData.attackTrigger, 0.1f);

        float delay = _boss.isPhaseTwo ? 0.4f / 1.5f : 0.4f;
        yield return new WaitForSeconds(delay);

        if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.attackRange + 0.5f)
        {
            enemy.playerHealth?.TakeDamage(15, DamageType.Physical);
        }
        yield return new WaitForSeconds(0.1f);
        _boss.PlayMeleeHitEffects();
        yield return new WaitForSeconds(0.6f);

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }
}