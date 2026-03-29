using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 position;
    public int health;
}

[Serializable]
public class EnemyData
{
    public string id; 
    public Vector3 position;
    public int health;
}

[Serializable]
public class GameData
{
    public PlayerData player;
    public List<EnemyData> enemies = new List<EnemyData>();
}