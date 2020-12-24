using UnityEngine;
using System;

[Serializable]
public class Spawn
{
    public GameObject spawnObject;
    public int count = 1;
    public float spawnTime = 0.0f;
    public int enemiesRemainingToSpawn = 0;
    public float spawnDelay = 0.0f;
    public float spawnTimer = 0.0f;
    public int maxEnemiesAlive = 4;

    [HideInInspector] public float spawnDelayTimer;
}
