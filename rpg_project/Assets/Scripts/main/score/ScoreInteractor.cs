using System;

public class ScoreInteractor : IScoreInteractor
{
    private readonly ScoreModel _model = new ScoreModel();
    private readonly EnemyKilledEventSO _deathEvent;

    public int CurrentScore => _model.CurrentScore;

    public event Action<int> OnScoreChanged
    {
        add => _model.OnScoreChanged += value;
        remove => _model.OnScoreChanged -= value;
    }

    public ScoreInteractor(EnemyKilledEventSO deathEvent)
    {
        _deathEvent = deathEvent;
        if (_deathEvent != null) _deathEvent.OnEnemyKilled += HandleEnemyKilled;
    }

    private void HandleEnemyKilled(EnemyBase enemy)
    {
        AddScore(enemy.ScoreReward);
    }

    public void AddScore(int amount)
    {
        _model.AddScore(amount);
    }

    public void SetScore(int value)
    {
        _model.SetScore(value);
    }

    public void Dispose()
    {
        if (_deathEvent != null)
        {
            _deathEvent.OnEnemyKilled -= HandleEnemyKilled;
        }
    }
}
