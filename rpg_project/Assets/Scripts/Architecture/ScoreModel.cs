using System;

public class ScoreModel
{
    private int _score;
    public int CurrentScore => _score;

    public event Action<int> OnScoreChanged;

    public void AddScore(int amount)
    {
        _score += amount;
        OnScoreChanged?.Invoke(_score);
    }

    public void SetScore(int value)
    {
        _score = value;
        OnScoreChanged?.Invoke(_score);
    }
}