using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackPattern : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyData;
    public GameObject bulletPrefab;         // Bullet prefab to instantiate
    public Transform firePoint;             // Point where bullets are fired from
    public int burstCount = 3;              // Number of bullets in a burst
    public float spreadAngle = 15f;         // Angle spread between bullets
    public float bulletSpeed = 10f;         // Speed of the bullets

    private float attackTimer;
    private Transform player;               // Reference to the player

    private void Start()
    {
        // Find the player once at the start
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        // Check if the enemy is ready to attack
        attackTimer += Time.deltaTime;
        if (player != null && attackTimer >= enemyData.FireRate)
        {
            if (Vector2.Distance(transform.position, player.position) <= enemyData.Range)
            {
                ShootBurst();
                attackTimer = 0f;
            }
        }
    }

    void ShootBurst()
    {
        // Calculate the direction to the player
        Vector2 targetDirection = (player.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Calculate the initial angle for the burst
        float initialAngle = baseAngle - spreadAngle * (burstCount - 1) / 2f;

        // Fire each bullet in the burst
        for (int i = 0; i < burstCount; i++)
        {
            float angle = initialAngle + i * spreadAngle;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            // Instantiate the bullet and set its direction and speed
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            Vector2 direction = bulletRotation * Vector2.right; // Right direction for 2D shooting
            rb.velocity = direction * bulletSpeed;

            // Set bullet damage if it has an EnemyProjectile component
            EnemyProjectile bulletScript = bullet.GetComponent<EnemyProjectile>();
            if (bulletScript != null)
            {
                bulletScript.damage = enemyData.Damage;
            }
            Destroy(bullet, 3f);
        }
    }
}
