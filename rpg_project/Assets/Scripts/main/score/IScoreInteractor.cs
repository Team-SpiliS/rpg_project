using System;

public interface IScoreInteractor
{
    int CurrentScore { get; }
    event Action<int> OnScoreChanged;
    void AddScore(int amount);
    void SetScore(int value);
}
