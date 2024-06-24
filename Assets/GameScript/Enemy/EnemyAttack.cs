using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float E_DamageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var HeathController = collision.gameObject.GetComponent<HeathController>();

            HeathController.TakeDamage(E_DamageAmount);
        }
    }
}
