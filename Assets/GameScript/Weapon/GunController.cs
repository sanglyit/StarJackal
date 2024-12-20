using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    [SerializeField] Transform shootingPoint;
    private float amountOfSpread;

    public int bulletsLeft;
    private ObjectPool bulletPool;

    private bool shooting;
    private bool reloading;
    private bool canShoot;

    //[SerializeField] bool isActiveWeapon;

    protected virtual void Start()
    {
        bulletsLeft = weaponData.MagSize;
        canShoot = true;
        bulletPool = ObjectPool.Instance;
    }

    protected virtual void Update()
    {
        if (weaponData.IsAutomatic)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < weaponData.MagSize && !reloading)
        {
            Reload();
        }

        if (weaponData.AutoReload && bulletsLeft == 0 && !reloading) 
        {
            Reload();
        }
            
        if (canShoot && shooting && !reloading && bulletsLeft > 0) 
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        canShoot = false;

        // Recoil Spread
        float spreadAngle = Random.Range(-weaponData.Spread, weaponData.Spread);
        Quaternion rotAfterSpread = Quaternion.Euler(0, 0, spreadAngle);

        // Calculate the direction with spread
        Vector3 spreadDirection = rotAfterSpread * shootingPoint.up;

        // Use the bullet prefab from the weapon data
        GameObject bullet = ObjectPool.Instance.GetFromPool(weaponData.BulletPrefab);
        if (bullet == null) return;

        // Update bullet data to match the current weapon's data
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetWeaponData(weaponData);
        }

        // Set bullet position and direction
        bullet.transform.position = shootingPoint.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.transform.up = spreadDirection;

        // Apply force to the bullet
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero; // Reset velocity in case it's a reused bullet
        bulletRb.AddForce(spreadDirection * weaponData.ShootForce, ForceMode2D.Impulse);

        bulletsLeft--;

        // Get reference to the player's PlayerStat component
        PlayerStat playerStats = GetComponentInParent<PlayerStat>();

        // Calculate the adjusted shooting cooldown using the method from PlayerStat
        float adjustedCooldown = playerStats.GetAdjustedCooldown(weaponData.ShootingCooldown);
        Invoke("ResetShot", adjustedCooldown);
    }

    protected virtual void ResetShot()
    {
        canShoot = true;
    }

    protected virtual void Reload ()
    {
        reloading = true;
        Invoke("FinishReload", weaponData.ReloadTime);
    }

    protected virtual void FinishReload()
    {
        reloading = false;
        bulletsLeft = weaponData.MagSize;
    }

}

