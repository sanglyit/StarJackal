using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    /*[SerializeField] int Damage;
    [SerializeField] string weaponName;

    [SerializeField] float shootingCooldown;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float shootForce;
    [SerializeField] float pierce;

    [SerializeField] int magSize;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool AutoReload;
    [SerializeField] GameObject bulletPrefab;
    */
    [SerializeField] Transform shootingPoint;
    [SerializeField] LayerMask  whatIsEnemy;
    private float amountOfSpread;

    private int bulletsLeft;
    private ObjectPool bulletPool;

    private bool shooting;
    private bool reloading;
    private bool canShoot;

    //[SerializeField] bool isActiveWeapon;

    protected virtual void Start()
    {
        bulletsLeft = weaponData.MagSize;
        canShoot = true;
        bulletPool = FindObjectOfType<ObjectPool>();
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

        // Spawn bullet
        // Get bullet from pool
        GameObject bulletCopy = Instantiate(weaponData.BulletPrefab);
        bulletCopy.transform.position = shootingPoint.position;
        bulletCopy.transform.rotation = Quaternion.identity;
        
        // Set bullet direction
        bulletCopy.transform.up = spreadDirection;

        // Apply force to the bullet
        Rigidbody2D bulletRb = bulletCopy.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero; // Reset velocity in case it's a reused bullet
        bulletRb.AddForce(spreadDirection * weaponData.ShootForce, ForceMode2D.Impulse);

        bulletsLeft--;
        Invoke("ResetShot", weaponData.ShootingCooldown);
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

