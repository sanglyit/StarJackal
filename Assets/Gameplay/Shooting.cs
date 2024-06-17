using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunPoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(0))
        {
            Shoot();
        }
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(gunPoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
