using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    // Current stats
    protected float currentDamage;
    protected float currentShootingCooldown;
    protected float currentPierce;
    protected float currentSpread;
    public GameObject hitEffect;
    private ObjectPool pool;

    public void SetWeaponData(WeaponScriptableObject newWeaponData)
    {
        weaponData = newWeaponData;
        currentDamage = weaponData.Damage;
        currentPierce = weaponData.Pierce;
        currentSpread = weaponData.Spread;
        ResetPierce();
    }


    private void OnEnable()
    {
        pool = ObjectPool.Instance; // Find the object pool in the scene
        ResetPierce();
    }

    void ResetPierce()
    {
        currentPierce = weaponData.Pierce; // Reset pierce to its base value
    }


    public float GetCurrentDamage()
    {
        return currentDamage * FindObjectOfType<PlayerStat>().CurrentStrength;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStat enemy = col.GetComponent<EnemyStat>();
            if (enemy != null)
            {
                enemy.TakeDamage(GetCurrentDamage());  // Apply damage
                PlayHitEffect();  // Play the particle effect
                ReducePierce();
            }
        }
        else if (col.GetComponent<Asteroid>())
        {
            Asteroid asteroid = col.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.TakeDamage(currentDamage); // Apply damage to asteroid
                PlayHitEffect();  // Play the particle effect
                ReducePierce();
            }
        }
    }

    private void OnBecameInvisible()
    {
        ObjectPool.Instance.ReturnObject(gameObject, weaponData.BulletPrefab); // Return bullet to the pool when it goes off screen
    }

    void ReducePierce()
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            ObjectPool.Instance.ReturnObject(gameObject, weaponData.BulletPrefab); // Return bullet to the pool when pierce is 0
        }
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            // Instantiate a new instance of the hitEffect prefab at the bullet's position
            GameObject instantiatedEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instantiatedEffect, 3f);
        }
    }
}

