using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    [SerializeField] float enemySpeed = 5f;
    [SerializeField] float rotateSpeed = 0.025f;
    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Try to target player
        if (!target)
        {
            GetTarget();
        } else {
            RotateTowardsTarget();
        }
        
    }

    private void FixedUpdate()
    {
        //Move forward till da player is destroyed
        rb.velocity = transform.up * enemySpeed;
    }
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(other.gameObject);
    //        target = null;
    //    } 
    //}
}
