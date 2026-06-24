using UnityEngine;

public class SaveInteractor : ISaveService
{
    private readonly ISaveRepository _repository;
    private readonly IScoreInteractor _scoreInteractor;
    private readonly IWorldStateApplier _worldStateApplier;

    public SaveInteractor(ISaveRepository repository, IScoreInteractor scoreInteractor, IWorldStateApplier worldStateApplier)
    {
        _repository = repository;
        _scoreInteractor = scoreInteractor;
        _worldStateApplier = worldStateApplier;
    }

    public bool HasSave() => _repository.Exists();

    public void SaveGame()
    {
        WorldSnapshot snapshot = new WorldSnapshot();
        snapshot.score = _scoreInteractor.CurrentScore;
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
            snapshot.enemies.Add(new EnemySnapshot
            {
                id = enemy.SaveId,
                position = enemy.transform.position,
                health = enemy.myHealth != null ? enemy.myHealth.GetCurrentHealth() : 100
            });

        }

        _repository.Save(snapshot);
    }

    public void LoadGame()
    {
        if (!_repository.Exists())
        {
            return;
        }

        WorldSnapshot snapshot = _repository.Load();

        if (snapshot != null)
        {
            if (_scoreInteractor != null)
            {
                _scoreInteractor.SetScore(snapshot.score);
            }

            _worldStateApplier?.Apply(snapshot);
        }
    }
}
