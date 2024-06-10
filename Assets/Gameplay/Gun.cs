using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Unity.PlasticSCM.Editor.WebApi;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    public float fireForce = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * fireForce, ForceMode2D.Impulse);
    }
}
