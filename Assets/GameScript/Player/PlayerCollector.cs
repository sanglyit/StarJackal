using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStat player;
    CircleCollider2D playerCollector;
    public float pullSpeed;
    void Start()
    {
        player = FindObjectOfType<PlayerStat>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        playerCollector.radius = player.currentMagnet;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.TryGetComponent(out ICollectable collectable))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);
            collectable.Collect();
        }
    }
}
