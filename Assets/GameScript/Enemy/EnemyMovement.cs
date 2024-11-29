using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public Transform target;

    public float detectionRange = 6.5f;     // Range within which the enemy will charge
    public float chargeSpeed = 12f;       // Speed during the charge
    public float chargeDuration = 1f;     // Duration of the charge
    public float chargeCooldown = 3f;     // Cooldown before the next charge

    private bool isCharging;
    private bool isWarning;
    public GameObject warningParticlePrefab;
    private Rigidbody2D rb;
    private EnemyStat enemy;

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
        // Try to acquire target if not already set
        if (target == null)
        {
            GetTarget();
        }
        // Rotate towards target if not charging
        if (!isCharging && target != null)
        {
            RotateTowardsTarget();
        }
        // Charge attack logic if conditions are met
        if (enemyData.CanCharge && !isCharging && !isWarning && target != null &&
            Vector2.Distance(transform.position, target.position) <= detectionRange)
        {
            StartCoroutine(ChargeAttack());
        }
    }

    private void FixedUpdate()
    {
        //Move forward till da player is destroyed
        if (!isCharging && target != null)
        {
            rb.velocity = transform.up * enemy.currentMoveSpeed;
        }

        else if (target == null || !target.gameObject.activeInHierarchy)
        {
            target = null;
            rb.velocity = Vector2.zero;
        }
    }
    private void RotateTowardsTarget()
    {
        if (isCharging) return;
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, enemyData.RotateSpeed);
    }

    private void GetTarget()
    {
        if (knockbackDuration > 0) return;

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

    private IEnumerator ChargeAttack()
    {
        isWarning = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        float originalMoveSpeed = enemy.currentMoveSpeed;
        enemy.currentMoveSpeed = 0f;

        GameObject instantiatedEffect = Instantiate(warningParticlePrefab, transform.position, Quaternion.identity);
        Destroy(instantiatedEffect, 1.5f);

        yield return new WaitForSeconds(1.5f); // Warning duration

        // Start charging toward the player
        isCharging = true;
        isWarning = false;
        rb.isKinematic = false;

        Vector2 chargeDirection = (target.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;

        yield return new WaitForSeconds(chargeDuration);

        // End charge
        rb.velocity = Vector2.zero;
        isCharging = false;
        enemy.currentMoveSpeed = originalMoveSpeed;

        // Cooldown before next charge
        yield return new WaitForSeconds(chargeCooldown);
    }

    public void KnockBack(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
