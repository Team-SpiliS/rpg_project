public class ScoreService : IScoreService
{
    private readonly ScoreModel _model = new ScoreModel();
    public int CurrentScore => _model.CurrentScore;

    public event System.Action<int> OnScoreChanged
    {
        add => _model.OnScoreChanged += value;
        remove => _model.OnScoreChanged -= value;
    }

    public void AddScore(int amount) => _model.AddScore(amount);

    public void SetScore(int value)
    {
        _model.SetScore(value);
    }
}