using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class JsonSaveRepository : ISaveRepository
{
    private string _path = Path.Combine(Application.persistentDataPath, "save.json");

    [System.Serializable]
    private class JsonSchema
    {
        public PlayerSnapshot playerData;
        public List<EnemySnapshot> enemiesData;
    }

    public void Save(WorldSnapshot snapshot)
    {
        JsonSchema schema = new JsonSchema
        {
            playerData = snapshot.player,
            enemiesData = snapshot.enemies
        };

        // 2. Сохраняем
        string json = JsonUtility.ToJson(schema, true);
        File.WriteAllText(_path, json);
        Debug.Log($"[JsonRepository] Сохранено в: {_path}");
    }

    public WorldSnapshot Load()
    {
        if (!Exists()) return null;

        string json = File.ReadAllText(_path);

        JsonSchema schema = JsonUtility.FromJson<JsonSchema>(json);

        return new WorldSnapshot
        {
            player = schema.playerData,
            enemies = schema.enemiesData
        };
    }

    public bool Exists() => File.Exists(_path);
}