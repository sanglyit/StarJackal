using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    //Current stats
    protected float currentDamage;
    protected float currentShootingCooldown;
    protected float currentPierce;
    protected float currentSpread;
    public GameObject hitEffect;
    private ObjectPool pool;

    private void OnEnable()
    {
        pool = FindObjectOfType<ObjectPool>(); // Find the object pool in the scene
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<EnemyMovement>())
    //    {
    //        var HealthController = collision.gameObject.GetComponent<HealthController>();
    //        if (HealthController != null)
    //        {
    //            HealthController.TakeDamage(Damage); // Apply damage
    //            pool.ReturnObject(gameObject); // Return bullet to the pool
    //        }
    //    }
    //    else if (collision.GetComponent<Asteroid>())
    //    {
    //        Asteroid asteroid = collision.GetComponent<Asteroid>();
    //        if (asteroid != null)
    //        {
    //            asteroid.Split(); // Split the asteroid
    //            pool.ReturnObject(gameObject); // Return bullet to the pool
    //        }
    //    }
    //}

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentPierce = weaponData.Pierce;
        currentSpread = weaponData.Spread;
        currentShootingCooldown = weaponData.ShootingCooldown;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStat enemy = col.GetComponent<EnemyStat>();
            enemy.TakeDamage(currentDamage);    //remember to use current damage
            ReducePierce();
        } 
        else if (col.GetComponent<Asteroid>())
        {
            Asteroid asteroid = col.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.Split(); // Split the asteroid
                ReducePierce();
            }
        }
    }
        private void OnBecameInvisible()
    {
        pool.ReturnObject(gameObject); // Return bullet to the pool when it goes off screen
    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            pool.ReturnObject(gameObject);
        }
    }
}
