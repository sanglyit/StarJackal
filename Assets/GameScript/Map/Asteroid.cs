using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int size = 3;  // Determines the size and health of the asteroid
    [SerializeField] private float friction = 0.98f;
    private Rigidbody2D rb;

    // HP system
    public float maxHealth = 20f;  // Max health of the asteroid
    private float currentHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial health based on size or another scaling factor
        currentHealth = maxHealth * size;

        // Set asteroid size and initial movement
        transform.localScale = 0.5f * size * Vector3.one;
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // Apply friction to gradually slow the asteroid down
        rb.velocity *= friction;
    }

    // Method to handle damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // If health reaches 0, destroy the asteroid
        if (currentHealth <= 0)
        {
            DestroyAsteroid();
        }
    }

    // Destroy the asteroid
    private void DestroyAsteroid()
    {
        // You can add explosion effects, sound, or any other logic here
        Destroy(gameObject);
    }
}
