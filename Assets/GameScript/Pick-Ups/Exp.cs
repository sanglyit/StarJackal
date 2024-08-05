using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : PickUp, ICollectable
{
    public int expGranted;
    public void Collect()
    {
        PlayerStat player = FindObjectOfType<PlayerStat>();
        player.IncreaseExperience(expGranted);
        return;
    }
    
}
