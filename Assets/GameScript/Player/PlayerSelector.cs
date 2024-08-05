using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector instance { get; private set; }
    public PlayerScriptableObject playerData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Debug.LogWarning("Extra " + this + " Deleted");
            Destroy(gameObject);
        }
    }
    public static PlayerScriptableObject GetData()
    {
        return instance.playerData;
    }

    public void SelectCharacter(PlayerScriptableObject player)
    {
        playerData = player;
    }
    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
