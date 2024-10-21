using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelector : MonoBehaviour
{
    public static ShipSelector instance;
    public PlayerScriptableObject shipData;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            //Debug.LogWarning("Extra " + this + " Deleted");
            Destroy(gameObject);
        }
    }

    public static PlayerScriptableObject GetData()
    {
        return instance.shipData;
    }

    public void SelectCharacter(PlayerScriptableObject ship)
    {
        shipData = ship;
    }
    public void DestroySingleTon()
    {
        instance = null;
        Destroy(gameObject);
    }
}
