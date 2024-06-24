using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovement>())
        {
            var HeathController = collision.gameObject.GetComponent<HeathController>();
                HeathController.TakeDamage(Damage); // Apply damage
                Destroy(gameObject); // Destroy bullet
        }
        
    }
}
