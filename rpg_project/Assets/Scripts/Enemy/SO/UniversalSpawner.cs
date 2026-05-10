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

    private int _deathCount = 0;
    private bool _bossIsActive = false;

    public GameObject bossPrefab;

    public WeaponConfigSO[] bossWeapons; 
    public ElementConfigSO[] bossElements;

    private void Start()
    {
        foreach (var factory in factories)
        {
            if (!_pools.ContainsKey(factory.prefab))
            {
                _pools.Add(factory.prefab, new Queue<EnemyBase>());

                for (int i = 0; i < prewarmEachFactory; i++)
                {
                    CreateNewInstanceForPool(factory);
                }
            }
        }

        for (int i = 0; i < maxEnemiesOnScene; i++)
        {
            SpawnRandomFromPool();
        }
    }

    private void CreateNewInstanceForPool(EnemyFactorySO factory)
    {
        EnemyBase enemy = factory.CreateInstance(transform.position);
        enemy.gameObject.SetActive(false);

        enemy.myHealth.OnDeath += () => HandleEnemyDeath(enemy, factory.prefab);

        _pools[factory.prefab].Enqueue(enemy);
    }

    public void SpawnRandomFromPool()
    {
        var randomFactory = factories[Random.Range(0, factories.Length)];
        GameObject targetPrefab = randomFactory.prefab;

        EnemyBase enemy;

        if (_pools[targetPrefab].Count > 0)
        {
            enemy = _pools[targetPrefab].Dequeue();
        }
        else
        {
            enemy = randomFactory.CreateInstance(transform.position);
            enemy.myHealth.OnDeath += () => HandleEnemyDeath(enemy, targetPrefab);
        }

        enemy.weaponConfig = randomFactory.weapon;
        enemy.elementConfig = randomFactory.element;

        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        enemy.transform.position = randomPos;
        enemy.gameObject.SetActive(true);
        enemy.ResetEnemy();
        enemy.ApplyVisuals();

        _currentActiveCount++;
    }

    private void HandleEnemyDeath(EnemyBase enemy, GameObject prefabKey)
    {
        _currentActiveCount--;
        if (!_bossIsActive)
        {
            _deathCount++;
            if (_deathCount >= 3)
            {
                SpawnBoss(); 
            }
        }


        StartCoroutine(RespawnRoutine(enemy, prefabKey));
    }

    private IEnumerator RespawnRoutine(EnemyBase enemy, GameObject prefabKey)
    {
        yield return new WaitForSeconds(respawnDelay);

        enemy.gameObject.SetActive(false);

        _pools[prefabKey].Enqueue(enemy);

        if (_currentActiveCount < maxEnemiesOnScene)
        {
            SpawnRandomFromPool();
        }
        if (!_bossIsActive && _currentActiveCount < maxEnemiesOnScene)
        {
            SpawnRandomFromPool();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }

    private void SpawnBoss()
    {
        _bossIsActive = true;

        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        BossEnemy boss = bossObj.GetComponent<BossEnemy>();
        if (boss != null)
        {
            boss.weaponConfig = bossWeapons[Random.Range(0, bossWeapons.Length)];

            boss.elementConfig = bossElements[Random.Range(0, bossElements.Length)];

            boss.ApplyVisuals();
        }
        boss.myHealth.OnDeath += HandleBossDeath;
    }

    private void HandleBossDeath()
    {

        _bossIsActive = false;

        _deathCount = 0;

        for (int i = 0; i < maxEnemiesOnScene; i++)
        {
            if (_currentActiveCount < maxEnemiesOnScene)
            {
                SpawnRandomFromPool();
            }
        }
    }
}