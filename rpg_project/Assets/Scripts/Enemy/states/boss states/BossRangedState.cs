using UnityEngine;
using System.Collections;

public class BossRangedState : BossState
{
    private Coroutine _shootCoroutine;

    private float _initialDelay = 0.6f;
    private float _burstInterval = 0.4f;
    private float _exitDelay = 0.5f;

    public BossRangedState(BossEnemy boss) : base(boss) { }

    public override void LogicUpdate()
    {
        if ((enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold) && enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(enemy.CreateFleeState());
        }
    }

    public override void Enter()
    {
        base.Enter();
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
        base.Exit();
    }

    private IEnumerator DoubleShootRoutine()
    {
        enemy.RotateTowardsPlayer();

        float speedMult = boss.isPhaseTwo ? 1.5f : 1f;
        enemy.animator.speed = speedMult;
        enemy.animator.CrossFade(enemy.animData.magicCast, 0.1f);

        yield return new WaitForSeconds(_initialDelay / speedMult);
        if (boss.IsDead) yield break;
        SpawnFireball();
        yield return new WaitForSeconds(_burstInterval / speedMult);
        if (boss.IsDead) yield break;
        SpawnFireball();
        yield return new WaitForSeconds(_exitDelay / speedMult);
        if (boss.IsDead) yield break;

        _shootCoroutine = null;
        enemy.StateMachine.ChangeState(boss.CreateChaseState());
    }

    private void SpawnFireball()
    {
        GameObject prefab = boss.GetCurrentProjectile();

        Transform spawnPoint = boss.CurrentShootPoint;

        if (prefab != null && spawnPoint != null)
        {
            GameObject proj = Object.Instantiate(prefab, spawnPoint.position, boss.transform.rotation);

            Vector3 targetPoint = enemy.player.position + Vector3.up * 1.2f;
            proj.transform.LookAt(targetPoint);

            if (proj.TryGetComponent(out MagicProjectile magic))
            {
                magic.Setup(boss.magicDamage, "Enemy");
            }
        }
    }
}
