public interface ISaveRepository
{
    void Save(GameData data);
    GameData Load();
    bool Exists();
}