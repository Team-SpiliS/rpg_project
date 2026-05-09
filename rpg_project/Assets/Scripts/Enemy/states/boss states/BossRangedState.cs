using UnityEngine;
using System.Collections;

public class BossRangedState : AbstractEnemyState
{
    private BossEnemy _boss;
    private Coroutine _shootCoroutine;

    private float _initialDelay = 0.6f;
    private float _burstInterval = 0.4f;
    private float _exitDelay = 0.5f;

    public BossRangedState(EnemyBase enemy) : base(enemy) { _boss = enemy as BossEnemy; }

    public override void LogicUpdate()
    {
        if (CheckGlobalTransitions()) return;

        if ((enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold) && enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(new EnemyFleeState(enemy));
        }
    }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
        }
        _shootCoroutine = enemy.StartCoroutine(DoubleShootRoutine());
    }

    public override void Exit()
    {
        if (_shootCoroutine != null) enemy.StopCoroutine(_shootCoroutine);
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
        enemy.animator.speed = 1f;
    }

    private IEnumerator DoubleShootRoutine()
    {
        enemy.RotateTowardsPlayer();

        float speedMult = _boss.isPhaseTwo ? 1.5f : 1f;
        enemy.animator.speed = speedMult;
        enemy.animator.CrossFade(enemy.animData.magicCast, 0.1f);

        yield return new WaitForSeconds(_initialDelay / speedMult);
        SpawnFireball();
        yield return new WaitForSeconds(_burstInterval / speedMult);
        SpawnFireball();
        yield return new WaitForSeconds(_exitDelay / speedMult);

        enemy.StateMachine.ChangeState(new BossChaseState(enemy));
    }

    private void SpawnFireball()
    {
        if (_boss.projectilePrefab && _boss.shootPoint)
        {
            GameObject proj = Object.Instantiate(_boss.projectilePrefab, _boss.shootPoint.position, _boss.transform.rotation);
            Vector3 targetPoint = enemy.player.position + Vector3.up * 1.2f;
            proj.transform.LookAt(targetPoint);

            if (proj.TryGetComponent(out MagicProjectile magic))
            {
                magic.Setup(_boss.magicDamage, "Enemy");
            }
        }
    }
}