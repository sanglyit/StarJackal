using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStat player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    { 
        //Apply the boost to the right stat in the child classes
    }
    void Start()
    {
        player = FindObjectOfType<PlayerStat>();
        ApplyModifier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
