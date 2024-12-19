using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float lifespan = 0.5f;
    public float pullSpeed;
    private PlayerStat target;
    private Vector2 initialPosition;

    [System.Serializable]
    public struct BobbingAnimation
    {
        public float frequency;
        public Vector2 direction;
    }
    public BobbingAnimation bobbingAnimation = new BobbingAnimation
    {
        frequency = 1.5f,
        direction = new Vector2(0, 0.3f)
    };

    [Header("Bonuses")]
    public int exp;
    public int health;
    public int gold;

    private bool isBeingPulled = false;

    protected virtual void Start()
    {
        initialPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (isBeingPulled && target != null)
        {
            // Pull towards the player
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, pullSpeed * Time.deltaTime * 0.07f);
        }
        else
        {
            // Bobbing animation
            transform.position = initialPosition + bobbingAnimation.direction * Mathf.Sin(Time.time * bobbingAnimation.frequency);
        }
    }

    public void SetTarget(PlayerStat player, float speed)
    {
        // Set the player as the target and start moving towards them
        target = player;
        pullSpeed = speed;
        isBeingPulled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the item has reached the player’s collection trigger
        if (col.CompareTag("CollectionTrigger") && col.GetComponentInParent<PlayerStat>() == target)
        {
            Collect();
        }
    }

    private void Collect()
    {
        // Apply the bonuses to the player
        if (exp != 0) target.IncreaseExperience(exp);
        if (health != 0) target.HealHealth(health);
        Destroy(gameObject);
    }
}
