using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class Medkit : MonoBehaviour, ICollectable
{
    public int healAmount;
    public void Collect()
    {
        PlayerStat player = FindObjectOfType<PlayerStat>();
        if (player.CurrentHealth < player.playerData.MaxHealth) 
        {
            player.HealHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
