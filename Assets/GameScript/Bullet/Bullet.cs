using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float Damage;
    private ObjectPool pool;

    private void OnEnable()
    {
        pool = FindObjectOfType<ObjectPool>(); // Find the object pool in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {
            var HeathController = collision.gameObject.GetComponent<HeathController>();
            if (HeathController != null)
            {
                HeathController.TakeDamage(Damage); // Apply damage
                pool.ReturnObject(gameObject); // Return bullet to the pool
            }
        }
    }
    private void OnBecameInvisible()
    {
        pool.ReturnObject(gameObject); // Return bullet to the pool when it goes off screen
    }

}
