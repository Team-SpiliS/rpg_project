public interface ISaveService
{
    void SaveGame();
    void LoadGame();
    bool HasSave();

    WorldSnapshot GetCurrentData();
}