using UnityEngine;

public class BossStateMachine : EnemyStateMachine
{
    private BossEnemy _boss;

    public BossStateMachine(BossEnemy boss) : base(boss)
    {
        _boss = boss;
    }

    
    public override IEnemyState CreateAttackState()
    {
        float dist = Vector3.Distance(_boss.transform.position, _boss.player.position);

        if (dist > _boss.attackRange + 1.5f)
            return new BossRangedState(_boss);

        return (Random.value > 0.6f)
            ? new BossHeavyAttackState(_boss)
            : new BossLightAttackState(_boss);
    }
}