using System.IO;
using UnityEngine;

public class JsonSaveRepository : ISaveRepository
{
    private string _path = Path.Combine(Application.persistentDataPath, "save.json");

    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_path, json);
        Debug.Log($"[Repository] Игра сохранена в: {_path}");
    }

    public GameData Load()
    {
        if (!Exists()) return null;
        string json = File.ReadAllText(_path);
        return JsonUtility.FromJson<GameData>(json);
    }

    public bool Exists() => File.Exists(_path);
}