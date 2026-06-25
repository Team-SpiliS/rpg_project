using UnityEngine;
using System.Collections;

public class EnemyAttackState : AbstractEnemyState
{
    private float _attackCooldown = 1.5f;
    private float _damageDelay = 0.4f; 

    private float _nextAttackTime = 0f;
    private Coroutine _attackCoroutine;
    private bool _isAttacking = false;

    public EnemyAttackState(EnemyBase enemy) : base(enemy) { }

    public override void Enter()
    {
        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
        }

        _nextAttackTime = Time.time;
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
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold &&
        enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(enemy.CreateFleeState());
            return;
        }

        if (_isAttacking) return;

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (dist > enemy.attackRange + 0.5f)
        {
            enemy.StateMachine.ChangeState(enemy.CreateChaseState());
            return;
        }

        if (Time.time >= _nextAttackTime)
        {
            _attackCoroutine = enemy.StartCoroutine(AttackRoutine());
            _nextAttackTime = Time.time + _attackCooldown; 
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        enemy.RotateTowardsPlayer();

        enemy.animator.CrossFade(enemy.animData.attackTrigger, 0.1f);

        yield return new WaitForSeconds(_damageDelay);

        float currentDist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (currentDist <= enemy.attackRange + 0.5f && enemy.playerHealth != null)
        {
            int damageFromWeapon = enemy.weaponConfig.baseDamage;
            enemy.playerHealth.TakeDamage(damageFromWeapon, DamageType.Physical);
        }

        yield return new WaitForSeconds(0.3f);

        enemy.animator.CrossFade(enemy.animData.idle, 0.2f);

        _isAttacking = false;
    }
}
