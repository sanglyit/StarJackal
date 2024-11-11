using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GuardianMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    public Transform target;
    private Rigidbody2D rb;

    private float spiralAngle = 0f;       // Track angle for spiral effect
    private float spiralRadius = 5f;      // Initial radius of the spiral

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Try to target player
        if (!target)
        {
            GetTarget();
        }
        else
        {
            // Rotate and move toward the player with a spiral effect
            RotateAndSpiralTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        // Apply movement in the spiraling direction
        if (target != null)
        {
            rb.velocity = transform.up * enemyData.EnemySpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if there's no target
        }
    }

    private void RotateAndSpiralTowardsTarget()
    {
        // Update the spiral angle and radius
        spiralAngle += enemyData.RotateSpeed * Time.deltaTime;
        spiralRadius = Mathf.Max(0f, spiralRadius - Time.deltaTime); // Gradually reduce radius

        // Calculate the offset from the target position for spiraling
        Vector3 spiralOffset = new Vector3(
            Mathf.Cos(spiralAngle) * spiralRadius,
            Mathf.Sin(spiralAngle) * spiralRadius,
            0);

        // Determine target position with the spiral effect
        Vector3 targetPosition = target.position + spiralOffset;

        // Calculate rotation toward the spiraling target position
        Vector2 targetDirection = targetPosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Rotate smoothly toward the calculated rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyData.RotateSpeed * Time.deltaTime);
    }

    private void GetTarget()
    {
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
}
