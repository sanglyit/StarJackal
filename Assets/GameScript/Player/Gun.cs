using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
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

    private bool shooting;
    private bool reloading;
    private bool canShoot;

    private void Start()
    {
        bulletsLeft = magSize;
        canShoot = true;
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
        GameObject bulletCopy = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        Destroy(bulletCopy, 4f);
        
        // Set bullet direction
        bulletCopy.transform.up = spreadDirection;

        // Apply force to the bullet
        bulletCopy.GetComponent<Rigidbody2D>().AddForce(spreadDirection * shootForce, ForceMode2D.Impulse);

        bulletsLeft--;
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
    }
}