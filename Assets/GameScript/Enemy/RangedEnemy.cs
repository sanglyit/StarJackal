using System.Collections;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;  // Holds enemy stats data
    public Transform target;                 // The player's transform
    public GameObject projectilePrefab;      // Projectile prefab to shoot
    public Transform firePoint;              // Point from which the projectile is fired

    private Rigidbody2D rb;
    private EnemyStat enemy;
    private float nextFireTime;

    private void Start()
    {
        enemy = GetComponent<EnemyStat>();
        rb = GetComponent<Rigidbody2D>();
        GetTarget();
    }

    private void Update()
    {
        // Find or rotate towards the player
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
            CheckAttackRange();  // Check if the enemy should shoot
        }
    }

    private void FixedUpdate()
    {
        // Move towards the player
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer > enemyData.Range)
            {
                rb.velocity = transform.up * enemy.currentMoveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;  // Stop moving if in attack range
            }
        }
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, enemyData.RotateSpeed);
    }

    private void GetTarget()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            PlayerStat playerStat = FindObjectOfType<PlayerStat>();
            if (playerStat != null)
            {
                target = playerStat.transform;
            }
        }
    }

    private void CheckAttackRange()
    {
        if (enemyData.Range > 0 && enemyData.FireRate > 0 && target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer <= enemyData.Range && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / enemyData.FireRate;
            }
        }
    }

    private void Shoot()
    {
        // Instantiate and shoot the projectile towards the player
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // Set the direction for the projectile
        Vector2 direction = (target.position - firePoint.position).normalized;
        projectileRb.velocity = direction * 10f;  // Adjust projectile speed as needed

        // Set the projectile damage if it has an EnemyProjectile component
        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = enemy.currentDamage;
        }
    }
}
