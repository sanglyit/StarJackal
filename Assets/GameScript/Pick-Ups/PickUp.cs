using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //protected bool hasCollected = false;
    public float lifespan = 0.5f;
    protected PlayerStat target;
    protected float speed;

    [Header("Bonuses")]
    public int exp;
    public int health;
    public int gold; 

    protected virtual void Update()
    {
        if (target)
        {  
            //Check distance between player and move item toward player
            Vector2 distance = target.transform.position - transform.position;
            if (distance.sqrMagnitude > speed * speed * Time.deltaTime) 
            {
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime;
            } else {
                Destroy(gameObject);
            }
        }
    }

    public virtual bool Collect(PlayerStat target, float speed, float lifespan = 0f)
    {
        if (!this.target) 
        {
            this.target = target;
            this.speed = speed;
            if (lifespan > 0) this.lifespan = lifespan;
            Destroy(gameObject, Mathf.Max(0.01f, this.lifespan));
            return true;
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        if (!target)
        {
            return; 
        }
        if (exp !=0)
        {
            target.IncreaseExperience(exp);
        }
        if (health != 0)
        {
            target.HealHealth(health);
        }
    }
}
