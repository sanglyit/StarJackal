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
        public int waveQuota; //the total number of enemies to spawn in a wave
        public float spawnInterval; //spawn delay
        public int spawnCount; //count enemy spawn
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //number of enemy to spawn in a wave
        public int spawnCount; //number of enemy type already spawned
        public GameObject enemyPrefab;
    }
    public List<Wave> waves; //wave list
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    public int maxEnemyAllowed;
    public float waveInterval;
    public int enemyAlive;
    float spawnTimer;
    public bool maxEnemyReached = false;
    bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; //Enemy spawn point list

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStat>().transform;
        CalculateWaveQuota();
        StartCoroutine(BeginNextWave());
    }
    void Update()
    {
        /*check if the wave has ended and the next wave should start
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }*/

        spawnTimer += Time.deltaTime;
        //Check time to spawn next enemy
        if (isWaveActive && spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }
    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);
        isWaveActive = true;
        CalculateWaveQuota();
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning($"Wave {currentWaveCount + 1} Quota: {currentWaveQuota}");
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
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemyAlive++;

                    if (enemyAlive >= maxEnemyAllowed)
                    {
                        maxEnemyReached = true;
                        return;
                    }
                }
            }
        }
        // Check if all enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
        {
            isWaveActive = false;
            currentWaveCount++;

            // If there are more waves, prepare the next wave
            if (currentWaveCount < waves.Count)
            {
                StartCoroutine(BeginNextWave());
            }
            else
            {
                Debug.Log("All waves complete!");
            }
        }
    }
    public void OnEnemyKilled()
    {
        enemyAlive--;
        //reset maxEnemiesReached flag if < max enemy amount
        if (enemyAlive < maxEnemyAllowed)
        {
            maxEnemyReached = false;
        }
    }
}
