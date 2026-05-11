public interface IScoreService
{
    int CurrentScore { get; }
    void AddScore(int amount);
    void SetScore(int value);
}