using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UniversalSpawner : MonoBehaviour
{
    public EnemyFactorySO[] factories;
    public int maxEnemiesOnScene = 3;
    public int prewarmEachFactory = 2;
    public float respawnDelay = 3f;
    public Vector3 areaSize = new Vector3(20, 0, 20);

    private Dictionary<GameObject, Queue<EnemyBase>> _pools = new Dictionary<GameObject, Queue<EnemyBase>>();
    private int _currentActiveCount = 0;
    public int deathCount { get; set; } = 0;
    private bool _bossIsActive = false;

    public BossFactorySO bossFactory;
    public EnemyKilledEventSO enemyKilledEvent;

    private BossEnemy _currentBoss;
    public bool isManualLoading;

    private void Start()
    {
        foreach (var factory in factories)
        {
            if (!_pools.ContainsKey(factory.prefab))
            {
                _pools.Add(factory.prefab, new Queue<EnemyBase>());
                for (int i = 0; i < prewarmEachFactory; i++) CreateNewInstanceForPool(factory);
            }
        }

        for (int i = 0; i < maxEnemiesOnScene; i++) SpawnRandomFromPool();
    }

    private void CreateNewInstanceForPool(EnemyFactorySO factory)
    {
        EnemyBase enemy = factory.CreateInstance(transform.position);
        enemy.gameObject.SetActive(false);
        InitializeSpawnHandle(enemy, factory.prefab);
        _pools[factory.prefab].Enqueue(enemy);
    }

    public void SpawnRandomFromPool()
    {
        var randomFactory = factories[Random.Range(0, factories.Length)];
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2), 0, Random.Range(-areaSize.z / 2, areaSize.z / 2));

        SpawnSpecific(randomFactory, randomPos, -1);
    }

    private void SpawnSpecific(EnemyFactorySO factory, Vector3 position, int hpOverride)
    {
        GameObject targetPrefab = factory.prefab;
        EnemyBase enemy;

        if (_pools.ContainsKey(targetPrefab) && _pools[targetPrefab].Count > 0)
        {
            enemy = _pools[targetPrefab].Dequeue();
        }
        else
        {
            enemy = factory.CreateInstance(position);
            InitializeSpawnHandle(enemy, targetPrefab);
        }

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.ResetEnemy();

        if (hpOverride > 0 && enemy.myHealth != null)
            enemy.myHealth.LoadHealth(hpOverride);

        _currentActiveCount++;
    }

    public void NotifyEnemyDeath(EnemyBase enemy, GameObject prefabKey)
    {
        if (enemyKilledEvent != null) enemyKilledEvent.RaiseEvent(enemy);

        _currentActiveCount--;


        if (!_bossIsActive)
        {
            deathCount++;
            if (deathCount >= 3)
            {
                SpawnBoss();
            }
        }
        StartCoroutine(RespawnRoutine(enemy, prefabKey));
    }

    private void InitializeSpawnHandle(EnemyBase enemy, GameObject prefabKey)
    {
        if (enemy == null) return;

        if (!enemy.TryGetComponent(out EnemySpawnHandle handle))
        {
            handle = enemy.gameObject.AddComponent<EnemySpawnHandle>();
        }

        handle.Initialize(this, enemy, prefabKey);
    }

    private IEnumerator RespawnRoutine(EnemyBase enemy, GameObject prefabKey)
    {
        yield return new WaitForSeconds(respawnDelay);

        enemy.gameObject.SetActive(false);
        if (_pools.ContainsKey(prefabKey)) _pools[prefabKey].Enqueue(enemy);

        if (!_bossIsActive && _currentActiveCount < maxEnemiesOnScene)
        {
            SpawnRandomFromPool();
        }
    }

    public void RestoreFromSave(List<EnemySnapshot> savedEnemies)
    {
        isManualLoading = true; 

        var activeEnemies = Object.FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var enemy in activeEnemies)
        {
            if (!enemy.CanReturnToPool)
            {
                Destroy(enemy.gameObject); 
            }
            else
            {
                enemy.gameObject.SetActive(false);
                if (enemy.originFactory != null && _pools.ContainsKey(enemy.originFactory.prefab))
                {
                    _pools[enemy.originFactory.prefab].Enqueue(enemy);
                }
            }
        }

        _currentActiveCount = 0;
        _bossIsActive = false;

        foreach (var snap in savedEnemies)
        {
            if (bossFactory != null && snap.id == bossFactory.BossId)
            {
                SpawnBoss(snap.position, snap.health);
            }
            else
            {
                var factory = System.Array.Find(factories, f => f.enemyId == snap.id);
                if (factory != null)
                {
                    SpawnSpecific(factory, snap.position, snap.health);
                }
            }
        }

        while (_currentActiveCount < maxEnemiesOnScene)
        {
            SpawnRandomFromPool();
        }


        isManualLoading = false; 
    }

    private void SpawnBoss()
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

        SpawnBoss(spawnPos, -1);
    }

    private void SpawnBoss(Vector3 position, int health = -1)
    {
        if (bossFactory == null)
        {
            return;
        }

        BossEnemy boss = bossFactory.Create(position);
        if (boss == null) return;

        _bossIsActive = true;
        _currentBoss = boss;

        if (boss.myHealth != null)
        {
            boss.myHealth.OnDeath += HandleBossDeath;

            if (health != -1)
            {
                boss.myHealth.LoadHealth(health);
            }
        }
    }

    private void HandleBossDeath()
    {
        if (enemyKilledEvent != null && _currentBoss != null) enemyKilledEvent.RaiseEvent(_currentBoss);

        _bossIsActive = false;
        deathCount = 0;
        _currentBoss = null;

        for (int i = 0; i < maxEnemiesOnScene; i++)
        {
            if (_currentActiveCount < maxEnemiesOnScene) SpawnRandomFromPool();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }

    public void SetDeathCount(int count)
    {
        deathCount = count;

    }
}
