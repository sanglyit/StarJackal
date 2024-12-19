using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStat player;
    protected Bullet bullet;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    { 
        //Apply the boost to the right stat in the child classes
    }
    void Start()
    {
        player = FindObjectOfType<PlayerStat>();
        bullet = GetComponent<Bullet>();
        ApplyModifier();
    }
    void Update()
    {
        
    }
}
