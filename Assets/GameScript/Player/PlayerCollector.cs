using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : MonoBehaviour
{
    public float pullSpeed;
    private PlayerStat player;
    private CircleCollider2D detector;

    private void Start()
    {
        player = GetComponentInParent<PlayerStat>();
    }

    public void SetRadius(float radius)
    {
        if (!detector) detector = GetComponent<CircleCollider2D>();
        detector.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out PickUp pickUp))
        {
            // Set the player as the target, enabling movement towards the player
            pickUp.SetTarget(player, pullSpeed);
        }
    }
}
