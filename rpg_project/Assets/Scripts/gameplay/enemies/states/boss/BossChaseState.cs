using UnityEngine;

public class BossChaseState : BossState
{
    private float _magicCheckTimer;
    private float _checkInterval = 2f;

    public BossChaseState(BossEnemy boss) : base(boss) { }

    public override void Enter()
    {
        base.Enter();
        enemy.animator.CrossFade(enemy.animData.chase, 0.2f);
        _magicCheckTimer = Time.time + _checkInterval;
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicUpdate()
    {
        if ((enemy.myHealth.GetCurrentHealth() < enemy.fleeHealthThreshold) && enemy.myHealth.GetCurrentHealth() > 0)
        {
            enemy.StateMachine.ChangeState(enemy.CreateFleeState());
            return;
        }

        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);

        if (Time.time > _magicCheckTimer)
        {
            _magicCheckTimer = Time.time + _checkInterval;
            if (dist < 15f && dist > enemy.attackRange + 2f && Random.value < 0.3f)
            {
                enemy.StateMachine.ChangeState(boss.CreateRangedState());
                return;
            }
        }

        if (dist <= enemy.attackRange)
        {
            enemy.StateMachine.ChangeState(CreateMeleeAttackState());
            return;
        }

        if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(enemy.player.position);


    }

    private IEnemyState CreateMeleeAttackState()
    {
        return Random.value > 0.6f
            ? boss.CreateHeavyAttackState()
            : boss.CreateLightAttackState();
    }
}
