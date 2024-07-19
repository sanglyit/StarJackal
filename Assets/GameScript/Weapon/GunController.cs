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

    [SerializeField] Text ammoDisplay;
    //[SerializeField] bool isActiveWeapon;

    protected virtual void Start()
    {
        bulletsLeft = weaponData.magSize;
        canShoot = true;
        bulletPool = FindObjectOfType<ObjectPool>();
        UpdateAmmoDisplay();
    }

    private void Update()
    {
        if (weaponData.isAutomatic)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        } else {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < weaponData.magSize && !reloading)
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

    private void Shoot()
    {
        canShoot = false;

        // Recoil Spread
        float spreadAngle = Random.Range(-weaponData.spread, weaponData.spread);
        Quaternion rotAfterSpread = Quaternion.Euler(0, 0, spreadAngle);

        // Calculate the direction with spread
        Vector3 spreadDirection = rotAfterSpread * shootingPoint.up;

        // Spawn bullet
        // Get bullet from pool
        GameObject bulletCopy = Instantiate(weaponData.bulletPrefab);
        bulletCopy.transform.position = shootingPoint.position;
        bulletCopy.transform.rotation = Quaternion.identity;
        
        // Set bullet direction
        bulletCopy.transform.up = spreadDirection;

        // Apply force to the bullet
        Rigidbody2D bulletRb = bulletCopy.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.zero; // Reset velocity in case it's a reused bullet
        bulletRb.AddForce(spreadDirection * weaponData.shootForce, ForceMode2D.Impulse);

        // Set bullet damage
        Bullet bulletComponent = bulletCopy.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Damage = weaponData.Damage;
        }

        bulletsLeft--;
        UpdateAmmoDisplay();
        Invoke("ResetShot", weaponData.shootingCooldown);
    }

    private void ResetShot()
    {
        canShoot = true;
    }

    private void Reload ()
    {
        reloading = true;
        Invoke("FinishReload", weaponData.reloadTime);
    }

    private void FinishReload()
    {
        reloading = false;
        bulletsLeft = weaponData.magSize;
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = $"{weaponData.weaponName}: {bulletsLeft}/{weaponData.magSize}";
            ammoDisplay.resizeTextForBestFit = true; 
            ammoDisplay.resizeTextMinSize = 5;
            ammoDisplay.resizeTextMaxSize = 150;
            ammoDisplay.gameObject.SetActive(true); //Show ammo display
        }
        else
        {
            ammoDisplay.gameObject.SetActive(false); // Hide unuse ammo display
        }
    }

}

