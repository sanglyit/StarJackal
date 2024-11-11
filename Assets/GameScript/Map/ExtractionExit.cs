using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify GameManager that the player has reached the extraction point
            GameManager.instance.CompleteExtraction();
        }
    }
}
