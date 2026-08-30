public interface ISaveRepository
{
    void Save(WorldSnapshot snapshot);

    WorldSnapshot Load();

    bool Exists();
}