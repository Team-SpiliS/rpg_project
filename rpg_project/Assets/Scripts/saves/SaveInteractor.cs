using UnityEngine;
using System.Collections.Generic;

public class SaveInteractor : ISaveService
{
    private readonly ISaveRepository _repository;
    private GameData _currentData;

    public SaveInteractor(ISaveRepository repository)
    {
        _repository = repository;
    }

    public bool HasSave() => _repository.Exists();

    public void SaveGame()
    {
        GameData data = new GameData();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            HealthComponent hc = playerObj.GetComponent<HealthComponent>();
            data.player = new PlayerData
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
                data.enemies.Add(new EnemyData
                {
                    id = enemy.transform.root.name,
                    position = enemy.transform.position,
                    health = hc.GetCurrentHealth()
                });
            }
        }

        _repository.Save(data);
        _currentData = data;

        Debug.Log($"[SaveInteractor] Сохранено объектов: {data.enemies.Count + 1}");
    }

    public void LoadGame()
    {
        _currentData = _repository.Load();
        if (_currentData != null)
        {
            Debug.Log("[SaveInteractor] Данные успешно загружены из файла.");
        }
    }

    public GameData GetCurrentData() => _currentData;
}