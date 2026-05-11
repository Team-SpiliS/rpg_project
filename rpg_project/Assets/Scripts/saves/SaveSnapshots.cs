using UnityEngine;
using System.Collections.Generic;

[System.Serializable] 
public class PlayerSnapshot
{
    public Vector3 position;
    public int health;
}

[System.Serializable]
public class EnemySnapshot
{
    public string id;
    public Vector3 position;
    public int health;
}
public class WorldSnapshot
{
    public PlayerSnapshot player;
    public List<EnemySnapshot> enemies = new List<EnemySnapshot>();
    public int score;
    public int deathCount;
}