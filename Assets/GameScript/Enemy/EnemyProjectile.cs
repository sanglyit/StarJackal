using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;                 // Damage the projectile deals
    public GameObject destroyEffect;     // Optional effect to play when destroyed
    public float lifetime = 2f;          // Time in seconds before the projectile is destroyed

    private void Start()
    {
        // Start the countdown to destroy the projectile after its lifetime expires
        Invoke(nameof(DestroyProjectile), lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if projectile hits the player
        if (collision.CompareTag("Player"))
        {
            PlayerStat player = collision.GetComponent<PlayerStat>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            DestroyProjectile();
        }
        if (collision.CompareTag("Prop"))
        {
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(damage);
            }
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        // Play the destroy effect if set
        if (destroyEffect != null)
        {
            GameObject instantiatedEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(instantiatedEffect, 3f);
        }
        Destroy(gameObject);  // Destroy the projectile itself
    }
}
