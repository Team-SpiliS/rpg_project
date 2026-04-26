using UnityEngine;
using System.Collections;

public class BossHeavyAttackState : AbstractEnemyState
{
    private BossEnemy _boss;
    private Coroutine _attackRoutine;

    public BossHeavyAttackState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

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
        enemy.animator.CrossFade(enemy.animData.heavyAttack, 0.2f);

        float delay = _boss.isPhaseTwo ? 0.7f / 1.5f : 0.7f;
        yield return new WaitForSeconds(delay);

        float currentDist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (currentDist <= enemy.attackRange + 0.5f && enemy.playerHealth != null)
        {
            enemy.playerHealth.TakeDamage(_boss.heavyDamage, DamageType.Physical);
        }

        yield return new WaitForSeconds(1.0f);
        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }
}