using System;

public class ScoreService : IScoreService
{
    private int _score;
    public int CurrentScore => _score;
    private readonly EnemyKilledEventSO _deathEvent;

    public ScoreService(EnemyKilledEventSO deathEvent)
    {
        _deathEvent = deathEvent;
        if (_deathEvent != null) _deathEvent.OnEnemyKilled += HandleEnemyKilled;
    }

    private void HandleEnemyKilled(EnemyBase enemy)
    {
        int points = (enemy is BossEnemy) ? 100 : 10;
        AddScore(points);
    }

    public void AddScore(int amount)
    {
        _score += amount;
    }

    public void SetScore(int value)
    {
        _score = value;

    }
}