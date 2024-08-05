using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class Medkit : PickUp, ICollectable
{
    public int healAmount;
    public void Collect()
    {
        PlayerStat player = FindObjectOfType<PlayerStat>();
        player.HealHealth(healAmount);
        return;
    }
}
