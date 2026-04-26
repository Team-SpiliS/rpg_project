using UnityEngine;
using System.Collections;

public class EnemyRangedAttackState : AbstractEnemyState
{
    private float _fireRate = 3f; 
    private float _castDelay = 0.3f; 

    private float _nextFireTime = 0f;
    private Coroutine _attackCoroutine;
    private bool _isAttacking = false;
    private RangedEnemy _mage;

    public EnemyRangedAttackState(EnemyBase enemy) : base(enemy)
    {
        _mage = enemy as RangedEnemy;
    }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
        }
        _nextFireTime = Time.time; 
    }

    public override void Exit()
    {
        if (_attackCoroutine != null)
        {
            enemy.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
        _isAttacking = false;
        if (enemy.agent.isOnNavMesh) enemy.agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold)
        {
            enemy.StateMachine.ChangeState(new EnemyFleeState(enemy));
            return;
        }

        if (_isAttacking) return;

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist < _mage.retreatDistance || dist > enemy.attackRange + 0.5f)
        {
            enemy.StateMachine.ChangeState(new EnemyChaseState(enemy));
            return;
        }

        if (Time.time >= _nextFireTime)
        {
            _attackCoroutine = enemy.StartCoroutine(ShootRoutine());
            _nextFireTime = Time.time + _fireRate;
        }
        else
        {
            enemy.RotateTowardsPlayer();
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isAttacking = true;
        enemy.RotateTowardsPlayer();

        enemy.animator.CrossFade(enemy.animData.attackTrigger, 0.1f);

        yield return new WaitForSeconds(_castDelay);

        if (_mage.projectilePrefab && _mage.shootPoint)
        {
            GameObject proj = Object.Instantiate(_mage.projectilePrefab, _mage.shootPoint.position, enemy.transform.rotation);

            Vector3 targetPoint = enemy.player.position + Vector3.up * 1.2f;
            proj.transform.LookAt(targetPoint);

            if (proj.TryGetComponent(out MagicProjectile magic))
            {
                magic.Setup(_mage.magicDamage, "Enemy");
            }
        }

        yield return new WaitForSeconds(0.5f);

        enemy.animator.CrossFade(enemy.animData.idle, 0.2f);
        _isAttacking = false;
    }
}