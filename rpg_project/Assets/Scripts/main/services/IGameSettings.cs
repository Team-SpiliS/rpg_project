public interface IGameSettings
{
    bool IsPeacefulMode { get; set; }
}

public class GameSettingsService : IGameSettings
{
    public bool IsPeacefulMode { get; set; } = false; 
}