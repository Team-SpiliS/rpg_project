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
        public int scoreData;
        public int deathCountData;
    }

    public void Save(WorldSnapshot snapshot)
    {
        JsonSchema schema = new JsonSchema
        {
            playerData = snapshot.player,
            enemiesData = snapshot.enemies,
            scoreData = snapshot.score,
            deathCountData = snapshot.deathCount
        };

        string json = JsonUtility.ToJson(schema, true);
        File.WriteAllText(_path, json);
    }

    public WorldSnapshot Load()
    {
        if (!Exists()) return null;

        string json = File.ReadAllText(_path);

        JsonSchema schema = JsonUtility.FromJson<JsonSchema>(json);

        return new WorldSnapshot
        {
            player = schema.playerData,
            enemies = schema.enemiesData,
            score = schema.scoreData,
            deathCount = schema.deathCountData
        };
    }

    public bool Exists() => File.Exists(_path);
}