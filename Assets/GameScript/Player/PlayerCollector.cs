using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : MonoBehaviour
{
    PlayerStat player;
    CircleCollider2D detector;
    public float pullSpeed;
    private void Start()
    {
        player = GetComponentInParent<PlayerStat>();
        
    }
    public void SetRadius(float r)
    {
        if(!detector) detector = GetComponent<CircleCollider2D>();
        detector.radius = r;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //check if the Gobject has Icollectable interface
        if(col.TryGetComponent(out PickUp collectable))
        {
            
            collectable.Collect(player, pullSpeed);
        }
    }
}
