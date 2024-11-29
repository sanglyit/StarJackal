using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can trigger the portal
        { 
            SceneManager.LoadScene("GameBattleBoss"); // Replace with the name of your boss scene
        }
    }
}
