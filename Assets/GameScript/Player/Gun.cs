using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    [SerializeField] int Damage;

    [SerializeField] float shootingCooldown;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float shootForce;

    [SerializeField] int magSize;
    [SerializeField] bool isAutomatic;
    [SerializeField] bool AutoReload;

    [SerializeField] Transform shootingPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LayerMask  whatIsEnemy;

    private float amountOfSpread;

    private int bulletsLeft;
    private ObjectPool bulletPool;

    private bool shooting;
    private bool reloading;
    private bool canShoot;
    public Text ammoDisplay;

    private void Start()
    {
        bulletsLeft = magSize;
        canShoot = true;
        bulletPool = FindObjectOfType<ObjectPool>();
        UpdateAmmoDisplay();
    }

    private void Update()
    {
        if (isAutomatic)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Reload();
        }

        if (AutoReload && bulletsLeft == 0 && !reloading) 
        {
            Reload();
        }
            
        if (canShoot && shooting && !reloading && bulletsLeft > 0) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        canShoot = false;

        // Recoil Spread
        float spreadAngle = Random.Range(-spread, spread);
        Quaternion rotAfterSpread = Quaternion.Euler(0, 0, spreadAngle);

        // Calculate the direction with spread
        Vector3 spreadDirection = rotAfterSpread * shootingPoint.up;

        // Spawn bullet
        // Get bullet from pool
        GameObject bulletCopy = bulletPool.GetObject();
        bulletCopy.transform.position = shootingPoint.position;
        bulletCopy.transform.rotation = Quaternion.identity;
        
        // Set bullet direction
        bulletCopy.transform.up = spreadDirection;

        // Apply force to the bullet
        Rigidbody2D bulletRb = bulletCopy.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero; // Reset velocity in case it's a reused bullet
        bulletRb.AddForce(spreadDirection * shootForce, ForceMode2D.Impulse);

        // Set bullet damage
        Bullet bulletComponent = bulletCopy.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Damage = Damage;
        }

        bulletsLeft--;
        UpdateAmmoDisplay();
        Invoke("ResetShot", shootingCooldown);
    }

    private void ResetShot()
    {
        canShoot = true;
    }

    private void Reload ()
    {
        reloading = true;
        Invoke("FinishReload", reloadTime);
    }

    private void FinishReload()
    {
        reloading = false;
        bulletsLeft = magSize;
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{bulletsLeft}/{magSize}";
        }
    }
}
