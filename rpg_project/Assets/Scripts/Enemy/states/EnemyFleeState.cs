using UnityEngine;

public class EnemyFleeState : AbstractEnemyState
{
    public EnemyFleeState(EnemyBase enemy) : base(enemy) { }

    public override void Enter()
    {
        if (HasParameter(enemy.animator, enemy.animData.attackTrigger))
        {
            enemy.animator.ResetTrigger(enemy.animData.attackTrigger);
        }
        enemy.animator.ResetTrigger("Hit"); 

        enemy.animator.speed = 1.3f;

        enemy.animator.Play(enemy.animData.flee);
    }
    public override void LogicUpdate()
    {
        Vector3 runDirection = (enemy.transform.position - enemy.player.position).normalized;
        Vector3 targetPos = enemy.transform.position + runDirection * 10f;

        if (enemy.agent.isOnNavMesh) enemy.agent.SetDestination(targetPos);

        if (Vector3.Distance(enemy.transform.position, enemy.player.position) > 20f)
        {
            enemy.wasHitByPlayer = false;
            enemy.StateMachine.ChangeState(new EnemyIdleState(enemy));
        }
    }

    public override void Exit()
    {
        enemy.animator.speed = 1.0f;
    }
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}