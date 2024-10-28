using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public Transform target;
    //[SerializeField] private float enemySpeed = 5f;
    //[SerializeField] private float rotateSpeed = 0.025f;
    private Rigidbody2D rb;
    EnemyStat enemy;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    
    private void Start()
    {
        enemy = GetComponent<EnemyStat>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
            return; // Skip further logic while knockback is active
        }
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
        if (target != null)
        {
            rb.velocity = transform.up * enemy.currentMoveSpeed;
        }

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = null;
            rb.velocity = Vector2.zero;
            return;
        }
    }
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, enemyData.RotateSpeed);
    }

    private void GetTarget()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
            return; // Skip targeting while in knockback
        }

        // Cache player transform if not already cached or player is inactive
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            PlayerStat playerStat = FindObjectOfType<PlayerStat>();
            if (playerStat != null)
            {
                target = playerStat.transform;
            }
        }
    }

    public void KnockBack(Vector2 velocity, float duration)
    {
        //ignore knockback if the duration is greater than 0
        if(knockbackDuration > 0) return;

        //begin knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
