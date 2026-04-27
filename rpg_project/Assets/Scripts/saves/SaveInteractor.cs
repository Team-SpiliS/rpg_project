using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveService
{
    private readonly ISaveRepository _repository;
    private WorldSnapshot _currentData;

    public SaveInteractor(ISaveRepository repository)
    {
        _repository = repository;
    }

    public bool HasSave() => _repository.Exists();

    public void SaveGame()
    {
        WorldSnapshot snapshot = new WorldSnapshot();

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

        GameObject[] enemiesOnScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemiesOnScene)
        {
            HealthComponent hc = enemy.GetComponent<HealthComponent>();
            if (hc == null) hc = enemy.GetComponentInChildren<HealthComponent>();

            if (hc != null)
            {
                snapshot.enemies.Add(new EnemySnapshot
                {
                    id = enemy.transform.root.name,
                    position = enemy.transform.position,
                    health = hc.GetCurrentHealth()
                });
            }
        }

        _repository.Save(snapshot);
        _currentData = snapshot;

    }

    public void LoadGame()
    {
        _currentData = _repository.Load();
        if (_currentData != null)
        {
        }
    }

    public WorldSnapshot GetCurrentData() => _currentData;
}