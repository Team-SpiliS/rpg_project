using UnityEngine;

public class BasicEnemy : EnemyBase
{
    protected override void Start()
    {
        base.Start(); 

        StateMachine.Initialize(new EnemyIdleState(this));
    }
}