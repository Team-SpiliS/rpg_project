using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Настройки Мага")]
    public float retreatDistance = 5f;
    public int magicDamage = 25;
    public GameObject projectilePrefab;
    public Transform shootPoint;

    public override AbstractEnemyState CreateAttackState()
    {
        return new EnemyRangedAttackState(this);
    }
}