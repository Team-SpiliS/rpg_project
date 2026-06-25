using UnityEngine;

public class EnemySpawnHandle : MonoBehaviour
{
    private UniversalSpawner spawner;
    private EnemyBase enemy;
    private GameObject prefabKey;
    private HealthComponent health;

    public void Initialize(UniversalSpawner owner, EnemyBase spawnedEnemy, GameObject spawnedPrefabKey)
    {
        Cleanup();

        spawner = owner;
        enemy = spawnedEnemy;
        prefabKey = spawnedPrefabKey;
        health = enemy != null ? enemy.myHealth : null;

        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    private void HandleDeath()
    {
        spawner?.NotifyEnemyDeath(enemy, prefabKey);
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        if (health != null)
        {
            health.OnDeath -= HandleDeath;
        }

        spawner = null;
        enemy = null;
        prefabKey = null;
        health = null;
    }
}
