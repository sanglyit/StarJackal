using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int size = 3; 
    [SerializeField] private float friction = 0.98f; 
    private Rigidbody2D rb;
    private void Start()
    {
        //Ti le dua tren kich thuoc
        transform.localScale = 0.5f * size * Vector3.one;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = new Vector2(Random.value, Random.value).normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction *  spawnSpeed, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        // Apply friction
        rb.velocity *= friction;
    }
    public void Split()
    {
        if (size > 1)
        {
            for (int i = 0; i < 2; i++)
            {
                Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
                newAsteroid.size = size - 1;
            }
        }
        Destroy(gameObject); // Destroy the original asteroid
    }

}
