using UnityEngine;

public class BossChaseState : AbstractEnemyState
{
    private BossEnemy _boss;
    private float _magicCheckTimer;
    private float _checkInterval = 2f; 

    public BossChaseState(EnemyBase enemy) : base(enemy)
    {
        _boss = enemy as BossEnemy;
    }

    public override void Enter()
    {
        enemy.animator.CrossFade(enemy.animData.chase, 0.2f);
        _magicCheckTimer = Time.time + _checkInterval;
    }

    public override void LogicUpdate()
    {
        if (enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold)
        {
            enemy.StateMachine.ChangeState(new EnemyFleeState(enemy));
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (Time.time > _magicCheckTimer)
        {
            _magicCheckTimer = Time.time + _checkInterval;

            if (dist < 15f && dist > enemy.attackRange + 2f)
            {
                if (Random.value < 0.3f) 
                {
                    enemy.StateMachine.ChangeState(new BossRangedState(enemy));
                    return;
                }
            }
        }

        if (dist <= enemy.attackRange)
        {
            enemy.StateMachine.ChangeState(enemy.CreateAttackState());
            return;
        }

        if (enemy.agent.isOnNavMesh)
        {
            enemy.agent.SetDestination(enemy.player.position);
        }
    }
}