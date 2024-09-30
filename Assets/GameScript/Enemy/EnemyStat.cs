using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    //current stats
    [SerializeField] public float currentMoveSpeed;
    [SerializeField] public float currentHealth;
    [SerializeField] public float currentDamage;

    public float despawnDistance = 30f;
    Transform player;
    private void Awake()
    {
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.EnemySpeed;
    }

    void Start()
    {
        PlayerStat playerStat = FindObjectOfType<PlayerStat>();
        if (playerStat != null)
        {
            player = playerStat.transform;  // Cache the player's transform
        }
    }

    private void Update()
    {
        if (player == null) return;  // Check if the player is null before accessing

        // Check the distance from the player and despawn if needed
        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStat player = collision.gameObject.GetComponent<PlayerStat>();
            if (player != null)
            {
                player.TakeDamage(currentDamage);
            }
        }
    }
    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            es.OnEnemyKilled();
        }
        else
        {
            Debug.LogWarning("EnemySpawner not found in the scene. OnEnemyKilled was not called.");
        }
    }
    void ReturnEnemy()
    {
        // Make sure player is still valid before using its position
        if (player == null)
        {
            Debug.LogWarning("Player is null, unable to respawn enemy.");
            return;
        }

        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        if (es != null)
        {
            // Reposition the enemy relative to the player
            transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
        }
    }
}
