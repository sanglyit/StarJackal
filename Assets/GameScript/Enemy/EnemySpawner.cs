using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    private float _spawnTime;
    
    void Awake()
    {
        SetTimeUntilSpawn();
    }

    
    void Update()
    {
        _spawnTime -= Time.deltaTime;
        if (_spawnTime <= 0 ) 
        {
            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _spawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
    }

}
