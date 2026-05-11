using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveService
{
    private readonly ISaveRepository _repository;
    private readonly IScoreService _scoreService;
    private WorldSnapshot _currentData;

    public SaveInteractor(ISaveRepository repository, IScoreService scoreService)
    {
        _repository = repository;
        _scoreService = scoreService;
    }

    public bool HasSave() => _repository.Exists();

    public void SaveGame()
    {
        WorldSnapshot snapshot = new WorldSnapshot();
        snapshot.score = _scoreService.CurrentScore;
        var spawner = Object.FindAnyObjectByType<UniversalSpawner>();
        if (spawner != null)
        {
            snapshot.deathCount = spawner.deathCount;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            HealthComponent hc = playerObj.GetComponent<HealthComponent>();
            snapshot.player = new PlayerSnapshot
            {
                position = playerObj.transform.position,
                health = (hc != null) ? hc.GetCurrentHealth() : 100
            };
        }

        EnemyBase[] enemiesOnScene = Object.FindObjectsByType<EnemyBase>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var enemy in enemiesOnScene)
        {
            if (enemy.myHealth.GetCurrentHealth() <= 0) continue;
            string saveId;
            if (enemy is BossEnemy)
            {
                saveId = "Boss";
            }
            else
            {

                saveId = enemy.originFactory != null ? enemy.originFactory.enemyId : "Unknown";
            }
            snapshot.enemies.Add(new EnemySnapshot
            {
                id = saveId,
                position = enemy.transform.position,
                health = enemy.myHealth != null ? enemy.myHealth.GetCurrentHealth() : 100
            });

        }

        _repository.Save(snapshot);
        _currentData = snapshot;
    }

    public void LoadGame()
    {

        if (!_repository.Exists())
        {
            return;
        }

        _currentData = _repository.Load();

        if (_currentData != null)
        {

            if (_scoreService != null)
            {
                _scoreService.SetScore(_currentData.score);
            }
        }
    }

    public WorldSnapshot GetCurrentData() => _currentData;
}