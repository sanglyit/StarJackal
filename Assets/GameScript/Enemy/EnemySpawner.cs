using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota; //the total number of enemies in a wave
        public float spawnInterval; //spawn delay
        public int spawnCount; //count enemy spawn
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //number of enemy in a wave
        public int spawnCount; //number of enemy type already spawned
        public GameObject enemyPrefab;
    }
    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    public int maxEnemyAllowed;
    public float spawnInterval;
    public int enemyAlive;
    float spawnTimer;
    public bool maxEnemyReached = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; //Enemy spawn point list

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStat>().transform;
        CalculateWaveQuota();
    }
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }
    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(spawnInterval);
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }
    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        //check minimum enemy spawned in wave
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemyReached)
        {
            //Spawn each type of enemy till it filled the quota
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // check minimum enemy type has spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemyAlive >= maxEnemyAllowed)
                    {
                        maxEnemyReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);
                    
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemyAlive++;
                }
            }
        }
        if (enemyAlive < maxEnemyAllowed)
        {
            maxEnemyReached = false;
        }
    }
    public void OnEnemyKilled()
    {
        enemyAlive--;
    }
}
