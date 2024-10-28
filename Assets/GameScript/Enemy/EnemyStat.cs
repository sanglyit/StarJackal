using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStat : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    //current stats
    [SerializeField] public float currentMoveSpeed;
    [SerializeField] public float currentHealth;
    [SerializeField] public float currentDamage;

    public float despawnDistance = 30f;
    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1); //color of the damage flash
    public float damageFlashDuration = 0.2f; //How long the flash last
    public float deathFadeTime = 0.5f; //The time it will fade away when ded
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement; 

    private void Awake()
    {
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
        currentMoveSpeed = enemyData.EnemySpeed;
    }

    void Start()
    {
        PlayerStat playerStat = FindObjectOfType<PlayerStat>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
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
    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackDuration = 0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        // create the text popup when the enemy takes damage
        if (dmg > 0)
        {
            GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }

        //Apply knockback if it is not zero
        if (knockbackForce > 0)
        {
            //Gets the direction of knockback
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.KnockBack(dir.normalized * knockbackForce, knockbackDuration);
        }

        
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor; 
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        //wait for a single frame
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        //This is a loop that fires every frame
        while(t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;
            //Set color for this frame
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }
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
