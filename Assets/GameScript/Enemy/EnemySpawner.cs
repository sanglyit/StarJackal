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
        public int waveQuota; // Total number of enemies to spawn in a wave
        public float spawnInterval; // Spawn delay
        public int spawnCount; // Count of enemies spawned so far
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; // Number of enemies to spawn in a wave
        public int spawnCount; // Number of this enemy type already spawned
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // List of waves
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    public int maxEnemyAllowed;
    public float waveInterval;
    public int enemyAlive;
    private float spawnTimer;
    private bool maxEnemyReached = false;
    private bool isWaveActive = false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; // Enemy spawn points

    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStat>().transform;
        CalculateWaveQuota();
        StartCoroutine(BeginNextWave());
    }

    void Update()
    {
        // Check time to spawn the next enemy
        spawnTimer += Time.deltaTime;
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
        ResetWaveSpawnCounts(); // Reset the spawn counts when starting a new wave
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
        // Check if the wave quota is met and max enemy count is not reached
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemyReached)
        {
            // Spawn each type of enemy until it fills the quota
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
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
        if (waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota && enemyAlive < 2)
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
                // If no more waves, repeat the last wave indefinitely
                currentWaveCount = waves.Count - 1;
                StartCoroutine(BeginNextWave());
            }
        }
    }

    // Reset the spawn counts for the current wave and its enemy groups
    void ResetWaveSpawnCounts()
    {
        waves[currentWaveCount].spawnCount = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            enemyGroup.spawnCount = 0;
        }
    }

    public void OnEnemyKilled()
    {
        enemyAlive--;

        // Reset maxEnemiesReached flag if less than max enemy amount
        if (enemyAlive < maxEnemyAllowed)
        {
            maxEnemyReached = false;
        }
    }
}
